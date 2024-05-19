using QuickOrderAtendimento.Domain.Entities;

namespace QuickOrderAtendimento.Domain.Adapters
{
    public interface IPedidoGateway : IBaseGateway, IBaseMongoDBRepository<Pedido>
    {
    }
}
