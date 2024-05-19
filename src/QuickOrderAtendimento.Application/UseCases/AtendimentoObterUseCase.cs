using QuickOrderAtendimento.Application.Dtos;
using QuickOrderAtendimento.Application.Dtos.Base;
using QuickOrderAtendimento.Application.UseCases.Interfaces;
using QuickOrderAtendimento.Domain.Adapters;
using QuickOrderAtendimento.Domain.Entities;
using QuickOrderAtendimento.Domain.Enums;

namespace QuickOrderAtendimento.Application.UseCases
{
    public class AtendimentoObterUseCase : IAtendimentoObterUseCase
    {
        private readonly IPedidoGateway _pedidoGateway;
        private readonly IPedidoStatusGateway _pedidoStatusGateway;

        public AtendimentoObterUseCase(IPedidoGateway pedidoGateway, IPedidoStatusGateway pedidoStatusGateway)
        {
            _pedidoGateway = pedidoGateway;
            _pedidoStatusGateway = pedidoStatusGateway;
        }

        public async Task<ServiceResult<PedidoDto>> ConsultarPedido(string codigoPedido)
        {
            var result = new ServiceResult<PedidoDto>();
            try
            {
                var pedido = await _pedidoGateway.Get(codigoPedido);


                var fila = await _pedidoStatusGateway.GetValue("CodigoPedido", codigoPedido);


                if (pedido == null || fila == null)
                {
                    result.AddError("Pedido não localizado");
                    return result;
                }

                var pedidoDto = new PedidoDto
                {
                    NumeroCliente = pedido.ClienteId,
                    CodigoPedido = pedido.Id.ToString(),
                    DataHoraInicio = pedido.DataHoraInicio,
                    DataHoraFinalizado = pedido.DataHoraFinalizado,
                    Observacao = pedido.Observacao,
                    PedidoPago = pedido.PedidoPago,
                    ValorPedido = pedido.ValorPedido,
                    ProdutosPedido = SetListaProdutos(pedido?.Produtos),
                    StatusPedido = fila.StatusPedido,
                };

                result.Data = pedidoDto;
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }
            return result;
        }

        public async Task<ServiceResult<List<PedidoDto>>> ConsultarFilaPedidos()
        {
            var result = new ServiceResult<List<PedidoDto>>();
            try
            {
                var pedido = _pedidoGateway.GetAll().Result;

                if (pedido == null || pedido.Count() == 0 )
                {
                    result.AddError("Pedidos não localizado");
                    return result;
                }

                var fila = await _pedidoStatusGateway.GetAll();

                fila = fila.Where(x => !x.StatusPedido.Equals(EStatusPedidoExtensions.ToDescriptionString(EStatusPedido.Pago))
                       && !x.StatusPedido.Equals(EStatusPedidoExtensions.ToDescriptionString(EStatusPedido.PendentePagamento))
                       && !x.StatusPedido.Equals(EStatusPedidoExtensions.ToDescriptionString(EStatusPedido.Finalizado))
                       && !x.StatusPedido.Equals(EStatusPedidoExtensions.ToDescriptionString(EStatusPedido.CanceladoCliente))
                       && !x.StatusPedido.Equals(EStatusPedidoExtensions.ToDescriptionString(EStatusPedido.CanceladoAtendimento))
                       && !x.StatusPedido.Equals(EStatusPedidoExtensions.ToDescriptionString(EStatusPedido.PendenteCancelamento))
                       && !x.StatusPedido.Equals(EStatusPedidoExtensions.ToDescriptionString(EStatusPedido.Criado)))
                        .OrderByDescending(x => (int)(EStatusPedido)Enum.Parse(typeof(EStatusPedido), x.StatusPedido)).OrderBy(x => x.DataAtualizacao);

                if (fila == null || fila.Count() == 0)
                {
                    result.AddError("Pedidos não localizado");
                    return result;
                }

                var listaPedidos = new List<PedidoDto>();

                foreach (var item in fila)
                {
                    var pedidoFila = pedido?.Where(x => x.Id.ToString().Equals(item.CodigoPedido)).FirstOrDefault();

                    if (pedidoFila == null)
                        result.Data = new List<PedidoDto>();

                    var pedidoDto = new PedidoDto
                    {
                        NumeroCliente = pedidoFila.ClienteId,
                        CodigoPedido = pedidoFila.Id.ToString(),
                        DataHoraInicio = pedidoFila.DataHoraInicio,
                        DataHoraFinalizado = pedidoFila.DataHoraFinalizado,
                        Observacao = pedidoFila.Observacao,
                        PedidoPago = pedidoFila.PedidoPago,
                        ValorPedido = pedidoFila.ValorPedido,
                        ProdutosPedido = SetListaProdutos(pedidoFila?.Produtos),
                        StatusPedido = item.StatusPedido,
                    };

                    listaPedidos.Add(pedidoDto);

                }
                result.Data = listaPedidos;
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }
            return result;
        }

        private List<ProdutoPedidoDto> SetListaProdutos(List<Produto> produto)
        {
            var listProdutoPedidoDto = new List<ProdutoPedidoDto>();
            if (produto != null)
            {
                foreach (var item in produto)
                {
                    var produtoPedidoDto = new ProdutoPedidoDto
                    {
                        NomeProduto = item.NomeProduto,
                        Quantidade = item.Quantidade,
                        Valor = item.ValorProduto
                    };
                    listProdutoPedidoDto.Add(produtoPedidoDto);
                }
            }
            return listProdutoPedidoDto;
        }
    }
}
