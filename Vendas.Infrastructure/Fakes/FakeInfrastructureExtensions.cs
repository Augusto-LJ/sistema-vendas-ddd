using Microsoft.Extensions.DependencyInjection;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Application.Commands.Pedidos.AdicionarItemAoPedido;
using Vendas.Application.Commands.PedidosCommands.CancelarPedido;
using Vendas.Application.Commands.PedidosCommands.CriarPedido;
using Vendas.Application.Commands.PedidosCommands.IniciarPagamento;
using Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEmSeparacao;
using Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEnviado;
using Vendas.Domain.Pedidos.Integration.Catalogo;
using Vendas.Domain.Pedidos.Integration.Cliente;

namespace Vendas.Infrastructure.Fakes;
public static class FakeInfrastructureExtensions
{
    public static IServiceCollection AddFakeInfrastructure(this IServiceCollection services)
    {
        // Repositórios
        services.AddSingleton<IPedidoRepository>(sp => sp.GetRequiredService<FakePedidoRepository>());
        services.AddSingleton<FakePedidoRepository>();

        // Gateways
        services.AddSingleton<ICatalogoGateway, FakeCatalogoGateway>();
        services.AddSingleton<IClientesGateway, FakeClientesGateway>();

        // ACLs
        services.AddSingleton<CatalogoAcl>();
        services.AddSingleton<ClientesAcl>();

        //Handlers
        services.AddScoped<CriarPedidoCommandHandler>();
        services.AddScoped<AdicionarItemAoPedidoCommandHandler>();
        services.AddScoped<IniciarPagamentoCommandHandler>();
        services.AddScoped<MarcarPedidoComoEmSeparacaoCommandHandler>();
        services.AddScoped<MarcarPedidoComoEnviadoCommandHandler>();
        services.AddScoped<MarcarPedidoComoEmSeparacaoCommandHandler>();
        services.AddScoped<CancelarPedidoCommandHandler>();

        return services;
    }
}
