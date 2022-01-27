using System;
using System.Net;
using System.Net.Http.Headers;
using System.Threading;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SystemGlobalServices_TestEx.Models;
using System.Collections.Generic;
using System.Linq;
using SystemGlobalServices_TestEx.Interfaces;

namespace SystemGlobalServices_TestEx.Services
{

    public class CbrDailyService : ICbrDaily
    {
        public static DateTime LastAccessToAPI { get; private set; }
        public static string DailyJsonStr { get; private set; } //Строка с сайта меняется раз в сутки
        public static CbrDailyModel DailyDeserializeStr { get; private set; }
        public static int CountValute { get; private set; }

        private static object _locker = new object();

        static CbrDailyService()
        {
            LastAccessToAPI = DateTime.MinValue;
            DailyJsonStr = "";
            DailyDeserializeStr = new CbrDailyModel();
        }

        public CbrDailyService()
        {

        }

        public Task<CbrDailyModel> GetCbrDailyAsync()
        {
            if ((DateTime.Now - LastAccessToAPI).Duration() > TimeSpan.FromHours(2))
            {
                UriBuilder builder = new UriBuilder();
                builder.Host = "www.cbr-xml-daily.ru";
                builder.Path = "daily_json.js";
                builder.Scheme = "https";

                using (System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient())
                {
                    if (Monitor.TryEnter(_locker))
                    {
                        try
                        {
                            DailyJsonStr = httpClient.GetStringAsync(builder.Uri).Result;
                            DailyDeserializeStr = JsonSerializer.Deserialize<CbrDailyModel>(DailyJsonStr);
                            LastAccessToAPI = DateTime.Now;
                            CountValute = DailyDeserializeStr.Valute.Count;
                        }
                        finally
                        {
                            Monitor.Exit(_locker);
                        }
                    }
                    
                }
            }
            return Task.FromResult(DailyDeserializeStr);
        }

        public async Task<IEnumerable<Valute>> GetItems(int curPage, int pageSize)
        {
            CbrDailyModel tmp = await GetCbrDailyAsync();

            IEnumerable<KeyValuePair<string, Valute>> tmp2 = tmp.Valute as IEnumerable<KeyValuePair<string, Valute>>;
            if (tmp2 == null)
            {
                tmp2 = new List<KeyValuePair<string, Valute>>();
                ((List<KeyValuePair<string, Valute>>)tmp2)
                    .Add(new KeyValuePair<string, Valute>("No data", GetNoDataValute()));
            }

            return tmp2.Skip((curPage - 1) * pageSize).Take(pageSize).Select(x => x.Value);
        }

        public async Task<Valute> GetItems(string id)
        {
            string idToUpper = id.ToUpper();
            CbrDailyModel tmp = await GetCbrDailyAsync(); //Обратились к API
                                                                        //Десериализовали данные, полученные через API

            IEnumerable<KeyValuePair<string, Valute>> tmp2 = (tmp.Valute as IEnumerable<KeyValuePair<string, Valute>>);
            if (tmp2 == null)
            {
                tmp2 = new List<KeyValuePair<string, Valute>>();
                ((List<KeyValuePair<string, Valute>>)tmp2)
                    .Add(new KeyValuePair<string, Valute>("No data", GetNoDataValute()));
            }

            return tmp2?.FirstOrDefault(x => x.Key == idToUpper ||
                                x.Value.ID == idToUpper ||
                                x.Value.NumCode == idToUpper ||
                                x.Value.CharCode == idToUpper ||
                                x.Value.Name.ToUpper() == idToUpper).Value ?? GetNoDataValute();
        }

        protected static Valute GetNoDataValute() => new Valute("No data", "No data", "No data",
                                                                -1, "No data".ToUpper(), -1, -1);
    }
}
