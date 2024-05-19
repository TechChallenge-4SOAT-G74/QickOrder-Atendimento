using QuickOrderAtendimento.Application.Dtos.Base;

namespace QuickOrderAtendimento.Application.UseCases.Interfaces
{
    public interface IAtendimentoAtualizarUseCase
    {
        Task<ServiceResult> AlterarStatusPedido(string codigoPedido, string pedidoStatus);
    }
}
