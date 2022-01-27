using SystemGlobalServices_TestEx.Interfaces;
using System.Collections.Generic;
using SystemGlobalServices_TestEx.Models;

namespace SystemGlobalServices_TestEx.Interfaces
{
    public interface IViewPageCbrModel
    {
        public IEnumerable<Valute> Valute { get; set; }
        public IPaginationCbrModel PageViewModel { get; set; }
    }
}