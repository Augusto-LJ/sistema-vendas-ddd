using Microsoft.Extensions.DependencyInjection;
using Vendas.Domain.Pedidos.Integration.Catalogo;
using Vendas.Domain.Pedidos.Integration.Cliente;
using Vendas.Infrastructure.Fakes;

namespace Vendas.Infrastructure.Extensions;
public static class IntegrationExtensions
{
    public static IServiceCollection AddFakeIntegration(this IServiceCollection services)
    {
        services.AddSingleton<ICatalogoGateway, FakeCatalogoGateway>();
        services.AddSingleton<IClientesGateway, FakeClientesGateway>();

        services.AddSingleton<CatalogoAcl>();
        services.AddSingleton<ClientesAcl>();

        return services;
    }
}
