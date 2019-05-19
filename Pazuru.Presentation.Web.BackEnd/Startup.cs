using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Pazuru.Mapping;
using Pazuru.Presentation.Web.BackEnd.Handlers;

namespace Pazuru.Presentation.Web.BackEnd
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices();
            services.AddWebSocketManager();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseWebSockets();
            app.MapWebSocketManager<PuzzleHandler>("/puzzle");
        }
    }
}
