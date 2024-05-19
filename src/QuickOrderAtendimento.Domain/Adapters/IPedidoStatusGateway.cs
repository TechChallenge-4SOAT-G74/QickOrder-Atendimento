using QuickOrderAtendimento.Domain.Entities;

namespace QuickOrderAtendimento.Domain.Adapters
{
    public interface IPedidoStatusGateway : IBaseGateway, IBaseMongoDBRepository<PedidoStatus>
    {
    }
}
