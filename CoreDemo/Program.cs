using Common;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CoreDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args);
            return builder.UseStartup<Startup>();
        }
    }
}
