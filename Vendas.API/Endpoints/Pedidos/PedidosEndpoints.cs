using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Application.Commands.Pedidos.AdicionarItemAoPedido;
using Vendas.Application.Commands.PedidosCommands.AdicionarItemAoPedido;
using Vendas.Application.Commands.PedidosCommands.CancelarPedido;
using Vendas.Application.Commands.PedidosCommands.CriarPedido;
using Vendas.Application.Commands.PedidosCommands.IniciarPagamento;
using Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEmSeparacao;
using Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEntregue;
using Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEnviado;
using Vendas.Application.Queries.Pedidos.ListarPedidosResumo;
using Vendas.Domain.Clientes;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Pedidos.Enums;
using Vendas.Infrastructure.Fakes;

namespace Vendas.API.Endpoints.Pedidos;

public static class PedidosEndpoints
{
    public static WebApplication MapPedidosEndpoints(this WebApplication app)
    {
        var pedidosGroup = app.MapGroup("/pedidos").WithTags("Pedidos");

        pedidosGroup.MapGet("/fake-ids", () => Results.Ok(new
        {
            clientes = new[]
            {
                new
                {
                    clienteId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    enderecos = new[]
                    {
                        new { enderecoId = Guid.Parse("33333333-0000-0000-0000-333333333333"), descricao = "Av. Paulista 1578, Bela Vista, São Paulo - SP" },
                        new { enderecoId = Guid.Parse("33333333-0000-0000-0000-444444444444"), descricao = "Rua das Flores 123, Centro, Rio de Janeiro - RJ" }
                    }

                },
                new
                {
                    clienteId = Guid.Parse("22222222-2222-3332-2111-222222222222"),
                    enderecos = new[]
                    {
                        new { enderecoId = Guid.Parse("33333333-0000-2220-0000-333333333333"), descricao = "Av. Paulista 999, Bela Vista, São Paulo - SP" },
                        new { enderecoId = Guid.Parse("33333333-0000-1110-0000-444444444444"), descricao = "Rua das Flores 784, Centro, Rio de Janeiro - RJ" }
                    }

                },
            },
            produtos = new[]
            {
                new { produtoId = Guid.Parse("55555555-0000-0000-0000-555555555555"), descricao = "Produto A" },
                new { produtoId = Guid.Parse("66666666-0000-0000-0000-666666666666"), descricao = "Produto B" },
                new { produtoId = Guid.Parse("77777777-0000-0000-0000-777777777777"), descricao = "Produto C" },
                new { produtoId = Guid.Parse("88888888-0000-0000-0000-888888888888"), descricao = "Produto D" }
            }
        })).WithSummary("Exibe os IDs dos dados disponíveis nos Fakes para usar nos testes").WithName("GetFakePedidoIds");


        pedidosGroup.MapGet("/", async ([FromServices] ListarPedidosResumoQueryHandler handler, CancellationToken cancellationToken) =>
        {
            var resultado = await handler.HandleAsync(new ListarPedidosResumoQuery(), cancellationToken);

            return Results.Ok(resultado);
        }).WithSummary("Lista todos os pedidos em memória");


        pedidosGroup.MapGet("/{id:guid}", async (Guid id, IPedidoRepository repository, CancellationToken cancellationToken) =>
        {
            var pedido = await repository.ObterPorIdAsync(id, cancellationToken);

            if (pedido is null)
                return Results.NotFound();

            var resultado = new
            {
                pedido.Id,
                pedido.NumeroPedido,
                pedido.ClienteId,
                pedido.ValorTotal,
                Status = pedido.StatusPedido.ToString(),
                pedido.DataCriacao,
                pedido.DataAtualizacao,
                Endereco = new
                {
                    pedido.EnderecoEntrega.Logradouro,
                    pedido.EnderecoEntrega.Numero,
                    pedido.EnderecoEntrega.Bairro,
                    pedido.EnderecoEntrega.Cidade,
                    pedido.EnderecoEntrega.Estado,
                    pedido.EnderecoEntrega.Cep
                },
                Itens = pedido.Itens.Select(i => new
                {
                    i.Id,
                    i.ProdutoId,
                    i.NomeProduto,
                    i.PrecoUnitario,
                    i.Quantidade,
                    i.ValorTotal
                }),
                Pagamentos = pedido.Pagamentos.Select(pg => new
                {
                    pg.Id,
                    Metodo = pg.MetodoPagamento.ToString(),
                    Status = pg.StatusPagamento.ToString(),
                    pg.Valor,
                    pg.CodigoTransacao,
                    pg.DataPagamento
                })
            };

            return Results.Ok(resultado);

        }).WithSummary("Retorna detalhes completos de um pedido filtrando por seu ID");


        pedidosGroup.MapPost("/", async (CriarPedidoRequest request, CriarPedidoCommandHandler handler, CancellationToken cancellationToken) => 
        {
            try
            {
                var command = new CriarPedidoCommand(request.ClienteId, request.EnderecoId);
                var result = await handler.HandleAsync(command, cancellationToken);

                return Results.Created($"/pedidos/{result.PedidoId}", result);
            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(new {erro = ex.Message});
            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new {erro = ex.Message});
            }
        }).WithSummary("Cria um novo pedido com os dados fornecidos");


        pedidosGroup.MapPost("/{id:guid}/itens", async (Guid id, AdicionarItemRequest request, AdicionarItemAoPedidoCommandHandler handler, CancellationToken cancellationToken) =>
        {
            try
            {
                var command = new AdicionarItemAoPedidoCommand(id, request.ProdutoId, request.Quantidade);

                var result = await handler.HandleAsync(command, cancellationToken);
                return Results.Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(new { erro = ex.Message });
            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new { erro = ex.Message });
            }
        }).WithSummary("Adiciona um item ao pedido");


        pedidosGroup.MapPost("/{id:guid}/pagamento", async (Guid id, IniciarPagamentoRequest request, IniciarPagamentoCommandHandler handler, CancellationToken cancellationToken) =>
        {
            try
            {
                var metodoPagamento = (MetodoPagamento)request.MetodoPagamento;
                var command = new IniciarPagamentoCommand(id, metodoPagamento);
                var result = await handler.HandleAsync(command, cancellationToken);
                return Results.Ok(result);
            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new { erro = ex.Message });
            }
        }).WithSummary("Inicia o processo de pagamento para um pedido");


        pedidosGroup.MapPost("/{id:guid}/pagamento/confirmacao", async (Guid id, ConfirmarPagamentoRequest request, IPedidoRepository repository, CancellationToken cancellationToken) =>
        {
            try
            {
                var pedido = await repository.ObterPorIdAsync(id, cancellationToken);

                if (pedido is null)
                    return Results.NotFound(new { erro = "Pedido não encontrado" });

                var pagamento = pedido.Pagamentos.FirstOrDefault(p => p.Id == request.PagamentoId);

                if (pagamento is null)
                    return Results.NotFound(new { erro = "Pagamento não encontrado para este pedido" });

                // Simula a confirmação do pagamento
                pagamento.GerarCodigoTransacaoLocal();
                pagamento.ConfirmarPagamento();
                pedido.HandlePagamentoAprovado(pagamento.Id);

                await repository.AtualizarAsync(pedido, cancellationToken);

                return Results.Ok(new
                {
                    PedidoId = pedido.Id,
                    PagamentoId = pagamento.Id,
                    StatusPedido = pedido.StatusPedido.ToString(),
                    StatusPagamento = pagamento.StatusPagamento.ToString(),
                    pagamento.CodigoTransacao
                });

            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new { erro = ex.Message });
            }
        }).WithSummary("Confirma o pagamento do pedido (simula gateway)");


        pedidosGroup.MapPost("/{id:guid}/pagamento/separacao", async (Guid id, MarcarPedidoComoEmSeparacaoCommandHandler handler, CancellationToken cancellationToken) =>
        {
            try
            {
                var command = new MarcarPedidoComoEmSeparacaoCommand(id);
                var result = await handler.HandleAsync(command, cancellationToken);
                return Results.Ok(result);
            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new { erro = ex.Message });
            }
        }).WithSummary("Marca o pedido como \"Em separação\"");


        pedidosGroup.MapPost("/{id:guid}/pagamento/enviado", async (Guid id, MarcarPedidoComoEnviadoCommandHandler handler, CancellationToken cancellationToken) =>
        {
            try
            {
                var command = new MarcarPedidoComoEnviadoCommand(id);
                var result = await handler.HandleAsync(command, cancellationToken);
                return Results.Ok(result);
            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new { erro = ex.Message });
            }
        }).WithSummary("Marca o pedido como \"Enviado\"");


        pedidosGroup.MapPost("/{id:guid}/pagamento/entregue", async (Guid id, MarcarPedidoComoEntregueCommandHandler handler, CancellationToken cancellationToken) =>
        {
            try
            {
                var command = new MarcarPedidoComoEntregueCommand(id);
                var result = await handler.HandleAsync(command, cancellationToken);
                return Results.Ok(result);
            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new { erro = ex.Message });
            }
        }).WithSummary("Exclui um pedido por ID");


        pedidosGroup.MapPost("/{id:guid}/cancelar", async (Guid id, CancelarPedidoRequest? request, CancelarPedidoCommandHandler handler, CancellationToken cancellationToken) =>
        {
            try
            {
               var command = new CancelarPedidoCommand(id, request?.CodigoMotivo ?? "Outro");
               var result = await handler.HandleAsync(command, cancellationToken);
                return Results.Ok(result);
            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new { erro = ex.Message });
            }
        }).WithSummary("Cancela um pedido");

        return app;
    }
}
