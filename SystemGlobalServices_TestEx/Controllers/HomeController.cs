using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SystemGlobalServices_TestEx.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using SystemGlobalServices_TestEx.Services;
using System.Text.Json.Serialization;
using SystemGlobalServices_TestEx.Interfaces;

namespace SystemGlobalServices_TestEx.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICbrDaily _cbrDailyService;

        public HomeController(ILogger<HomeController> logger, ICbrDaily cbrDailyServ)
        {
            _logger = logger;
            _cbrDailyService = cbrDailyServ;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Currency(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Currencies");

            return View(await _cbrDailyService.GetItems(id));
        }

        [HttpGet]
        public async Task<IActionResult> Currencies(string id = null, int curPage = 1, int pageSize = 15)
        {
            if (!string.IsNullOrEmpty(id))
                return RedirectToAction("Currency", new { id = id });

            Task<IEnumerable<Valute>> items = _cbrDailyService.GetItems(curPage, pageSize);
            int count = (await _cbrDailyService.GetCbrDailyAsync()).Valute.Count;

            IPaginationCbrModel pageViewModel = new PaginationCbrModel(count, curPage, pageSize);
            IViewPageCbrModel dailyViewPageModel = new ViewPageCbrModel()
            {
                PageViewModel = pageViewModel,
                Valute = await items,
            };
            ViewBag.PageSize = pageSize;
            return View(dailyViewPageModel);
        }

        public async Task<IActionResult> Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
