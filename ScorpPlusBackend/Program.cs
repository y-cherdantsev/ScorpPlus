using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ScorpPlusBackend
{
    /// <summary>
    /// Entry class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// API configurations
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Entry point of an application
        /// </summary>
        /// <param name="args">Array of program arguments</param>
        public static async Task Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "ScorpPlus";
            await CreateHostBuilder(args).Build().RunAsync();
        }

        /// <summary>
        /// Creates host builder
        /// </summary>
        /// <param name="args">Array of program arguments</param>
        /// <returns>HostBuilder</returns>
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}