using QuickOrderAtendimento.Domain.Adapters;
using QuickOrderAtendimento.Domain.Entities;
using QuickOrderAtendimento.Infra.Gateway.Core;

namespace QuickOrderAtendimento.Infra.Gateway
{
    public class PedidoGateway : BaseMongoDBRepository<Pedido>, IPedidoGateway
    {
        public PedidoGateway(IMondoDBContext mondoDBContext) : base(mondoDBContext)
        {
        }
    }
}
