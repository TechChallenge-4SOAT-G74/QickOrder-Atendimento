using QuickOrderAtendimento.Domain.Entities;

namespace QuickOrderAtendimento.Domain.Adapters
{
    public interface ICarrinhoGateway : IBaseGateway, IBaseMongoDBRepository<Carrinho>
    {
    }
}
