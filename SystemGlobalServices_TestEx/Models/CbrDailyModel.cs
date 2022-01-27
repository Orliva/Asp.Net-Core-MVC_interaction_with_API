using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace SystemGlobalServices_TestEx.Models
{
    public class CbrDailyModel
    {
        public DateTime Date { get; set; }
        public DateTime PreviousDate { get; set; }
        public string PreviousURL { get; set; }
        public DateTime Timestamp { get; set; }
        public ConcurrentDictionary<string, Valute> Valute { get; set; }

        //public Dictionary<string, Valute> Valute { get; set; }
    }

    public class Valute
    {
        public string ID { get; set; }
        public string NumCode { get; set; }
        public string CharCode { get; set; }
        public int Nominal { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Previous { get; set; }

        [JsonConstructor]
        public Valute(string id, string numCode, string charCode,
            int nominal, string name, decimal value, decimal previous)
        {
            ID = id.ToUpper();
            NumCode = numCode.ToUpper();
            CharCode = charCode.ToUpper();
            Nominal = nominal;
            Name = name;
            Value = value;
            Previous = previous;
        }
    }

}
