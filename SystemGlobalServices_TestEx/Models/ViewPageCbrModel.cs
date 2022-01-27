using System.Collections.Generic;
using SystemGlobalServices_TestEx.Interfaces;

namespace SystemGlobalServices_TestEx.Models
{
    public class ViewPageCbrModel : IViewPageCbrModel
    {
        public IEnumerable<Valute> Valute { get; set; }
        public IPaginationCbrModel PageViewModel { get; set; }
    }
}
