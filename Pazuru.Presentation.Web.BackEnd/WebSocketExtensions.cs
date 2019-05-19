using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Pazuru.Presentation.Web.BackEnd
{
    public static class WebSocketExtensions
    {
        public static IApplicationBuilder MapWebSocketManager<TWebSocketHandler>(this IApplicationBuilder app, PathString path) where TWebSocketHandler: WebSocketHandler
        {
            return app.Map(path, self => self.UseMiddleware<WebSocketManagerMiddleware>(self.ApplicationServices.GetService<TWebSocketHandler>()));
        }

        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<WebSocketConnectionManager>();

            IEnumerable<Type> exportedTypes = Assembly.GetEntryAssembly()?.ExportedTypes;
            if (exportedTypes == null)
                return services;
            foreach (Type type in exportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }
    }
}