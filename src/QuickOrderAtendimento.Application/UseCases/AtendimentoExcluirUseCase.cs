using QuickOrderAtendimento.Application.Dtos.Base;
using QuickOrderAtendimento.Application.Events;
using QuickOrderAtendimento.Application.UseCases.Interfaces;
using QuickOrderAtendimento.Domain.Adapters;
using QuickOrderAtendimento.Domain.Entities;


namespace QuickOrderAtendimento.Application.UseCases
{
    public class AtendimentoExcluirUseCase : AtendimentoUseCaseBase, IAtendimentoExcluirUseCase
    {
        private readonly IPedidoGateway _pedidoGateway;
        private readonly IRabbitMqPub<Pedido> _rabbitMqPub;

        public AtendimentoExcluirUseCase(
            IPedidoStatusGateway pedidoStatusGateway,
            IPedidoGateway pedidoGateway,
            IRabbitMqPub<Pedido> rabbitMqPub) : base(pedidoStatusGateway)
        {
            _pedidoGateway = pedidoGateway;
            _rabbitMqPub = rabbitMqPub;
        }

        public async Task<ServiceResult> CancelarPedido(string codigoPedido, string statusPedido)
        {
            var result = new ServiceResult();
            try
            {
                var pedido = await _pedidoGateway.Get(codigoPedido);

                if (pedido == null)
                {
                    result.AddError("Pedido não encontrado.");
                    return result;
                }

                pedido.StatusPedido = statusPedido;
                pedido.DataHoraFinalizado = DateTime.Now;

                _pedidoGateway.Update(pedido);

                await AlterarStatusPedidoBase(codigoPedido, statusPedido);

                _rabbitMqPub.Publicar(pedido, "Pedido", "Pedido_Cancelado");
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }
            return result;
        }

    }
}
