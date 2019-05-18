using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Pazuru.Mapping;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace Pazuru.Presentation.Web.BackEnd
{
    internal class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseWebSockets();

            app.Use(async (http, next) =>
            {
                if (http.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await http.WebSockets.AcceptWebSocketAsync();
                    if (webSocket != null && webSocket.State == WebSocketState.Open)
                    {
                        // TODO: Handle the socket here.
                        byte[] buffer = Encoding.Default.GetBytes("HELLLOOOOOOOOOO");
                        await webSocket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
                    }
                }
                else
                {
                    // If not a websocket request pass down to normal mvc stuff  
                    await next();
                }
            });
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
