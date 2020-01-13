using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            await DownloadAsync();
        }

        static private async Task DownloadAsync()
        {

            var clinet = new HttpClient();
            await clinet.GetAsync("https://codehaks.com");

        }
    }
}
