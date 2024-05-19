using Microsoft.AspNetCore.Mvc;
using QuickOrderAtendimento.Application.UseCases.Interfaces;
using QuickOrderAtendimento.Domain.Enums;

namespace QuickOrderAtendimento.Api.Controllers
{
    public static class AtendimentAtendimentoController
    {
        public static void RegisterAtendimentoController(this WebApplication app)
        {
            app.MapGet("/consultarfilapedidos", async ([FromServices] IAtendimentoObterUseCase atendimentoObterUseCase) =>
            {
                return Results.Ok(await atendimentoObterUseCase.ConsultarFilaPedidos());
            });

            app.MapGet("/consultarpedido/{codigoPedido}", async ([FromServices] IAtendimentoObterUseCase atendimentoObterUseCase, string codigoPedido) =>
            {
                return Results.Ok(await atendimentoObterUseCase.ConsultarPedido(codigoPedido));
            });

            app.MapPut("/alterarstatuspedido/{id}", async ([FromServices] IAtendimentoAtualizarUseCase atendimentoAtualizarUseCase, string codigoPedido, string statusPedido) =>
            {
                return Results.Ok(await atendimentoAtualizarUseCase.AlterarStatusPedido(codigoPedido, statusPedido));
            });


            app.MapDelete("/cancelarpedido/{codigoPedido}", async ([FromServices] IAtendimentoExcluirUseCase atendimentoExcluirUseCase, string codigoPedido) =>
            {
                return Results.Ok(await atendimentoExcluirUseCase.CancelarPedido(codigoPedido, EStatusPedido.CanceladoCliente.ToString()));
            });
        }
    }
}


