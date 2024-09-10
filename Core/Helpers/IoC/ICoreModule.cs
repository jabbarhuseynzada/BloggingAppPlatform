using Microsoft.Extensions.DependencyInjection;

namespace Core.Helpers.IoC;

public interface ICoreModule
{
    void Load(IServiceCollection serviceCollection);
}
