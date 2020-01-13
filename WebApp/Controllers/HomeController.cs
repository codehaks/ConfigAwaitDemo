using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("api/download")]
        public async Task<IActionResult> Download()
        {
            var t1 = Thread.CurrentThread.ManagedThreadId;
            var result=await DownloadAsync();
            var t2 = Thread.CurrentThread.ManagedThreadId;
            return Ok( $"{t1} ---> ["+result+$"] ---> {t2}");
        }

        static private async Task<string> DownloadAsync()
        {
            var t1 = Thread.CurrentThread.ManagedThreadId;
            await Task.Delay(1000);
            var t2 = Thread.CurrentThread.ManagedThreadId;

            return $" {t1} -> {t2}";
        }

        public IActionResult Privacy()
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
