using QuickOrderAtendimento.Application.Dtos.Base;
using QuickOrderAtendimento.Domain.Entities;

namespace QuickOrderAtendimento.Application.UseCases.Interfaces
{
    public interface IPedidoCriarUseCase
    {
        Task<ServiceResult> CriarPedido(int numeroCliente, ProdutoCarrinho? produtoCarrinho = null);
    }
}
