
using QuickOrderAtendimento.Application.Dtos.Base;
using QuickOrderAtendimento.Application.UseCases.Interfaces;
using QuickOrderAtendimento.Domain.Adapters;
using QuickOrderAtendimento.Domain.Enums;

namespace QuickOrderAtendimento.Application.UseCases
{
    public class AtendimentoAtualizarUseCase : AtendimentoUseCaseBase, IAtendimentoAtualizarUseCase
    {
        private readonly IPedidoGateway _pedidoGateway;

        public AtendimentoAtualizarUseCase(
                IPedidoStatusGateway pedidoStatusGateway,
                IPedidoGateway pedidoGateway) : base(pedidoStatusGateway)
        {
            _pedidoGateway = pedidoGateway;
        }

        public async Task<ServiceResult> AlterarStatusPedido(string codigoPedido, string pedidoStatus)
        {
            var result = new ServiceResult();
            try
            {
                var pedido = await _pedidoGateway.Get(codigoPedido);

                if (pedido == null)
                {
                    result.AddError("Pedido não localizado.");
                    return result;
                }

                await AlterarStatusPedidoBase(pedido.Id.ToString(), EStatusPedidoExtensions.ToDescriptionString(EStatusPedido.PendentePagamento));


            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }
            return result;
        }
    }
}
