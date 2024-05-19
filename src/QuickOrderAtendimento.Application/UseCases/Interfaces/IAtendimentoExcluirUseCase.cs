using QuickOrderAtendimento.Application.Dtos.Base;

namespace QuickOrderAtendimento.Application.UseCases.Interfaces
{
    public interface IAtendimentoExcluirUseCase
    {
        Task<ServiceResult> CancelarPedido(string codigoPedido, string statusPedido);
    }
}
