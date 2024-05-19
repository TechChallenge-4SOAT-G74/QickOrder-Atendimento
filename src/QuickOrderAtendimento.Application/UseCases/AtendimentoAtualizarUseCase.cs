
using QuickOrderAtendimento.Application.Dtos.Base;
using QuickOrderAtendimento.Application.UseCases.Interfaces;
using QuickOrderAtendimento.Domain.Adapters;
using QuickOrderAtendimento.Domain.Entities;
using QuickOrderAtendimento.Domain.Enums;
using QuickOrderAtendimento.Infra.MQ;

namespace QuickOrderAtendimento.Application.UseCases
{
    public class AtendimentoAtualizarUseCase : AtendimentoUseCaseBase, IAtendimentoAtualizarUseCase
    {
        private readonly IPedidoGateway _pedidoGateway;
        private readonly IRabbitMqPub<Pedido> _rabbitMqPub;

        public AtendimentoAtualizarUseCase(
                IPedidoStatusGateway pedidoStatusGateway,
                IPedidoGateway pedidoGateway,
                IRabbitMqPub<Pedido> rabbitMqPub) : base(pedidoStatusGateway)
        {
            _pedidoGateway = pedidoGateway;
            _rabbitMqPub = rabbitMqPub;
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

                _rabbitMqPub.Publicar(pedido, "Pedido", "Pedido_Atendimento");

            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }
            return result;
        }
    }
}
