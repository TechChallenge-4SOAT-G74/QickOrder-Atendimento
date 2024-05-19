namespace QuickOrderAtendimento.Domain.Entities
{
    public class Pedido : EntityMongoBase
    {
        public Pedido(
                     DateTime dataHoraInicio,
                     DateTime? dataHoraFinalizado,
                     int clienteId,
                     string? statusPedido,
                     double valorPedido,
                     bool pedidoPago,
                     List<Produto>? produtos,
                     string? observacao = null)
        {
            DataHoraInicio = dataHoraInicio;
            DataHoraFinalizado = dataHoraFinalizado;
            ClienteId = clienteId;
            StatusPedido = statusPedido;
            Produtos = produtos;
            ValorPedido = valorPedido;
            PedidoPago = pedidoPago;
            Observacao = observacao;

        }
        public virtual DateTime DataHoraInicio { get; set; }
        public virtual DateTime? DataHoraFinalizado { get; set; }
        public virtual int ClienteId { get; set; }
        public virtual double ValorPedido { get; set; }
        public virtual string? Observacao { get; set; }
        public virtual bool PedidoPago { get; set; }
        public List<Produto>? Produtos { get; set; }
        public virtual string? StatusPedido { get; set; }

    }
}
