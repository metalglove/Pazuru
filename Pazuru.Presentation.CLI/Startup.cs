using Microsoft.Extensions.DependencyInjection;
using Pazuru.Mapping;
using System.Threading.Tasks;

namespace Pazuru.Presentation.CLI
{
    internal class Startup
    {
        private IGenericServiceProvider serviceProvider;

        internal Task IntializeAsync()
        {
            serviceProvider = new ServiceCollection()
                .AddApplicationServices()
                .BuildServiceProvider()
                .ToGenericServiceProvider();

            return Task.CompletedTask;
        } 

        internal IGenericServiceProvider GetServiceProvider()
        {
            return serviceProvider;
        }
    }
}
