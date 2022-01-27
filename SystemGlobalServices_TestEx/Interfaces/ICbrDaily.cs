using System;
using SystemGlobalServices_TestEx.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SystemGlobalServices_TestEx.Interfaces
{
    public interface ICbrDaily
    {
        static DateTime LastAccessToAPI { get; }
        static string DailyJsonStr { get; }
        static CbrDailyModel DailyDeserializeStr { get; }
        static int CountValute { get; }
        Task<CbrDailyModel> GetCbrDailyAsync();
        Task<IEnumerable<Valute>> GetItems(int curPage, int pageSize);
        Task<Valute> GetItems(string id);
    }
}
