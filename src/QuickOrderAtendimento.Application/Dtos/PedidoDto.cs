﻿namespace QuickOrderAtendimento.Application.Dtos
{
    public class PedidoDto
    {
        public string? CodigoPedido { get; set; }
        public DateTime? DataHoraInicio { get; set; }
        public DateTime? DataHoraFinalizado { get; set; }
        public int NumeroCliente { get; set; }
        public double ValorPedido { get; set; }
        public string? Observacao { get; set; }
        public bool PedidoPago { get; set; }
        public string? StatusPedido { get; set; }
        public List<ProdutoPedidoDto>? ProdutosPedido { get; set; }

    }
}
