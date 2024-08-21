using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Threading;
using System.Threading.Tasks;

namespace TeamServer.Models.Listeners

{
    public class HttpListener : Listener
    {

        public override string Name { get; }

        public int BindPort { get; }

        private CancellationTokenSource _tokenSource;

        public HttpListener(string name, int binPort)
        {
            Name = name;
            BindPort = binPort;
        }

        public override async Task Start()
        {
            var hostBuildler = new HostBuilder()
                .ConfigureWebHostDefaults(host =>
                {
                    host.UseUrls($"http://0.0.0.0:{BindPort}");
                    host.Configure(ConfigureApp);
                    host.ConfigureServices(ConfigureServices);
                });

            var host = hostBuildler.Build();
            _tokenSource = new CancellationTokenSource();
            host.RunAsync(_tokenSource.Token);
        }
        private void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSingleton(AgentService);
        }

        private void ConfigureApp(IApplicationBuilder app)
        {

            app.UseRouting();
            app.UseEndPoints(e =>
            {
                e.MapControllerRoute("/", "/", new { controller = "HttpListener", action = "HandleImplant" });

            });
        }

        public override void Stop()
        {
            _tokenSource.Cancel();
        }
    }
}
