using QuickOrderAtendimento.Application.Dtos.Base;

namespace QuickOrderAtendimento.Application.UseCases.Interfaces
{
    public interface IPedidoExcluirUseCase
    {
        Task<ServiceResult> CancelarPedido(string codigoPedido, string statusPedido);
        Task<ServiceResult> LimparCarrinho(string codigoPedido);
    }
}
