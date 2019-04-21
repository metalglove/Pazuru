using System;

namespace Pazuru.Mapping
{
    public interface IGenericServiceProvider : IServiceProvider
    {
        TService GetService<TService>();
    }
}
