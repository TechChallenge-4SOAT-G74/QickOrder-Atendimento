using QuickOrderAtendimento.Domain.Adapters;
using QuickOrderAtendimento.Domain.Entities;
using QuickOrderAtendimento.Infra.Gateway.Core;

namespace QuickOrderAtendimento.Infra.Gateway
{
    public class CarrinhoGateway : BaseMongoDBRepository<Carrinho>, ICarrinhoGateway
    {
        public CarrinhoGateway(IMondoDBContext mondoDBContext) : base(mondoDBContext)
        {
        }
    }
}
