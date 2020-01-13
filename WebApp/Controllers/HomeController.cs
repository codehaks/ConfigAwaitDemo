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
            var task1 = DownloadAsync();
            var task2 = DownloadAsync();
            var task3 = DownloadAsync();

            var r = await Task.WhenAll(task1, task2, task3);
            var t2 = Thread.CurrentThread.ManagedThreadId;

            var result = $"{t1} ---> [" + r[0] + " --> " + r[1] + " --> " + r[2] + $"] ---> {t2}";
            _logger.LogInformation(result);
            return Ok(result);
        }

        [Route("api/download2")]
        public async Task<IActionResult> Download2()
        {
            var t1 = Thread.CurrentThread.ManagedThreadId;
            var r1 = await DownloadAsync().ConfigureAwait(false);
            var r2 = await DownloadAsync().ConfigureAwait(false);

 
            var t2 = Thread.CurrentThread.ManagedThreadId;

            var result = $"{t1} ---> [" + r1 + " --> " + r2+ $"] ---> {t2}";
            _logger.LogInformation(result);
            return Ok(result);
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
