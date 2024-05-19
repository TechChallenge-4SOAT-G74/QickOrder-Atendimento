using QuickOrderAtendimento.Application.Dtos.Base;
using QuickOrderAtendimento.Domain.Adapters;

namespace QuickOrderAtendimento.Application.UseCases
{
    public class AtendimentoUseCaseBase
    {
        private readonly IPedidoStatusGateway _pedidoStatusGateway;

        public AtendimentoUseCaseBase(IPedidoStatusGateway pedidoStatusGateway)
        {
            _pedidoStatusGateway = pedidoStatusGateway;
        }

        public async Task<ServiceResult> AlterarStatusPedidoBase(string codigoPedido, string pedidoStatus)
        {
            var result = new ServiceResult();
            try
            {
                var pedido = await _pedidoStatusGateway.GetValue("CodigoPedido", codigoPedido);

                if (pedido == null)
                {
                    result.AddError("Pedido não localizado.");
                    return result;
                }

                pedido.StatusPedido = pedidoStatus;
                pedido.DataAtualizacao = DateTime.Now;
                _pedidoStatusGateway.Update(pedido);

            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }
            return result;
        }
    }
}