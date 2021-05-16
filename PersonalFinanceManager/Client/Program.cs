using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceManager.Client.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

#if DEBUG
            await Task.Delay(5000);
#endif

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.Build();
            await builder.Build().RunAsync();
        }
    }
}
