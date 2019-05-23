using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Register.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args);

            host.Run();
        }

        public static IWebHost CreateWebHostBuilder(string[] args){
            var builder = WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseIISIntegration()
                .CaptureStartupErrors(true)
                .UseStartup<Startup>()
                .Build();

            return builder;
        }
            
    }
}
