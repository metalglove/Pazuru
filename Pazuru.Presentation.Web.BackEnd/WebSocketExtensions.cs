using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Pazuru.Presentation.Web.BackEnd
{
    [ExcludeFromCodeCoverage]
    public static class WebSocketExtensions
    {
        public static IApplicationBuilder MapWebSocketManager<TWebSocketHandler>(this IApplicationBuilder app, PathString path) where TWebSocketHandler : WebSocketHandler
        {
            return app.Map(path, self => self.UseMiddleware<WebSocketManagerMiddleware>(self.ApplicationServices.GetService<TWebSocketHandler>()));
        }

        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<WebSocketConnectionManager>();

            IEnumerable<Type> exportedTypes = Assembly.GetEntryAssembly()?.ExportedTypes;
            if (exportedTypes == null)
                return services;

            exportedTypes
                .Where(type => type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                .ForEach(type => services.AddSingleton(type));
            return services;
        }
        private static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }
    }
}