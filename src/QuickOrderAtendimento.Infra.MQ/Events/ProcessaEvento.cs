using QuickOrderAtendimento.Application.UseCases.Interfaces;
using QuickOrderAtendimento.Domain.Adapters;
using QuickOrderAtendimento.Domain.Entities;
using System.Text.Json;

namespace QuickOrderAtendimento.Infra.MQ
{
    public class ProcessaEvento : IProcessaEvento
    {
        private readonly IPedidoGateway _pedidoGateway;

        public ProcessaEvento(IPedidoGateway pedidoGateway)
        {
            _pedidoGateway = pedidoGateway;
        }

        public void Processa(string mensagem)
        {
            var pedidoRead = JsonSerializer.Deserialize<Pedido>(mensagem);

            if (pedidoRead != null ) 
                _pedidoGateway.Create(pedidoRead);

        }
    }
}
