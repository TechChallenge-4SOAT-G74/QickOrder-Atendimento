using QuickOrderAtendimento.Application.Dtos;
using QuickOrderAtendimento.Application.Dtos.Base;

namespace QuickOrderAtendimento.Application.UseCases.Interfaces
{
    public interface IPedidoObterUseCase
    {
        Task<ServiceResult<PedidoDto>> ConsultarPedido(string id);
        Task<ServiceResult<List<PedidoDto>>> ConsultarListaPedidos();
    }
}
