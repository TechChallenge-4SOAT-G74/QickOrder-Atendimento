using QuickOrderAtendimento.Domain.Adapters;
using QuickOrderAtendimento.Domain.Entities;
using QuickOrderAtendimento.Infra.Gateway.Core;

namespace QuickOrderAtendimento.Infra.Gateway.Gateway
{
    public class PedidoStatusGateway : BaseMongoDBRepository<PedidoStatus>, IPedidoStatusGateway
    {
        public PedidoStatusGateway(IMondoDBContext mondoDBContext) : base(mondoDBContext)
        {
        }
    }
}
