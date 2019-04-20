using Microsoft.Extensions.DependencyInjection;
using Pazuru.Mapping;
using System;
using System.Threading.Tasks;

namespace Pazuru.Presentation.CLI
{
    internal class Startup
    {
        private IServiceProvider serviceProvider;

        internal Task IntializeAsync()
        {
            serviceProvider = new ServiceCollection()
                .AddApplicationServices()
                .BuildServiceProvider();

            return Task.CompletedTask;
        } 

        internal IServiceProvider GetServiceProvider()
        {
            return serviceProvider;
        }
    }
}
