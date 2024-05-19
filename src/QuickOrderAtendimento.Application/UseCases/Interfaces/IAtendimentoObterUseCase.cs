using QuickOrderAtendimento.Application.Dtos;
using QuickOrderAtendimento.Application.Dtos.Base;

namespace QuickOrderAtendimento.Application.UseCases.Interfaces
{
    public interface IAtendimentoObterUseCase
    {
        Task<ServiceResult<PedidoDto>> ConsultarPedido(string codigoPedido);
        Task<ServiceResult<List<PedidoDto>>> ConsultarFilaPedidos();
    }
}
