using Moq;
using QuickOrderAtendimento.Application.UseCases;
using QuickOrderAtendimento.Domain.Adapters;
using QuickOrderAtendimento.Domain.Entities;
using Xunit;

namespace QuickOrderAtendimento.Tests
{
    public class AtendimentoObterUseCaseTests
    {

        private readonly AtendimentoObterUseCase _atendimentoObterUseCase;
        private readonly Mock<IPedidoGateway> _pedidoGatewayMock;
        private readonly Mock<IPedidoStatusGateway> _pedidoStatusGatewayMock;

        public AtendimentoObterUseCaseTests()
        {
            _pedidoGatewayMock = new Mock<IPedidoGateway>();
            _pedidoStatusGatewayMock = new Mock<IPedidoStatusGateway>();
            _atendimentoObterUseCase = new AtendimentoObterUseCase(_pedidoGatewayMock.Object, _pedidoStatusGatewayMock.Object);
        }


        [Fact]
        public async Task ConsultarPedido_WhenPedidoAndFilaExist_ReturnsPedidoDto()
        {
            // Arrange
            var produto = new Produto("Lanche", "Produto 1", 1, 2, 10.0,
                new List<ProdutoItens> { new ProdutoItens { NomeProdutoItem = "Pão", Quantidade = 1, ValorItem = 10.0 } });
            var pedido = new Pedido(DateTime.Now, null, 1, "123", 10.0, true, new List<Produto> { produto }, null);

            var fila = new PedidoStatus(pedido.Id.ToString(), "EmPreparacao", DateTime.Now);

            _pedidoGatewayMock.Setup(g => g.Get(pedido.Id.ToString())).ReturnsAsync(pedido);

            _pedidoStatusGatewayMock.Setup(g => g.GetValue("CodigoPedido", pedido.Id.ToString())).ReturnsAsync(fila);

            // Act
            var result = await _atendimentoObterUseCase.ConsultarPedido(pedido.Id.ToString());

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(pedido.ClienteId, result.Data.NumeroCliente);
            Assert.Equal(pedido.Id.ToString(), result.Data.CodigoPedido);
            Assert.Equal(pedido.DataHoraInicio, result.Data.DataHoraInicio);
            Assert.Equal(pedido.DataHoraFinalizado, result.Data.DataHoraFinalizado);
            Assert.Equal(pedido.Observacao, result.Data.Observacao);
            Assert.Equal(pedido.PedidoPago, result.Data.PedidoPago);
            Assert.Equal(pedido.ValorPedido, result.Data.ValorPedido);
            // Assert other properties

            _pedidoGatewayMock.Verify(g => g.Get(pedido.Id.ToString()), Times.Once);
            _pedidoStatusGatewayMock.Verify(g => g.GetValue("CodigoPedido", pedido.Id.ToString()), Times.Once);
        }

        [Fact]
        public async Task ConsultarPedido_WhenPedidoOrFilaDoesNotExist_ReturnsError()
        {
            // Arrange
            var produto = new Produto("Lanche", "Produto 1", 1, 2, 10.0,
                new List<ProdutoItens> { new ProdutoItens { NomeProdutoItem = "Pão", Quantidade = 1, ValorItem = 10.0 } });
            var pedido = new Pedido(DateTime.Now, null, 1, "123", 10.0, true, new List<Produto> { produto }, null);

            var fila = new PedidoStatus(pedido.Id.ToString(), "EmPreparacao", DateTime.Now);

            _pedidoGatewayMock.Setup(g => g.Get(pedido.Id.ToString())).ReturnsAsync(pedido);


            // Act
            var result = await _atendimentoObterUseCase.ConsultarPedido("123");

            // Assert
            Assert.Null(result.Data);
            Assert.Contains("Pedido não localizado", result.Errors[0].Message);
        }


        [Fact]
        public async Task ConsultarFilaPedidos_WhenPedidoAndFilaExist_ReturnsPedidoDto()
        {
            // Arrange
            var produto = new Produto("Lanche", "Produto 1", 1, 2, 10.0,
                new List<ProdutoItens> { new ProdutoItens { NomeProdutoItem = "Pão", Quantidade = 1, ValorItem = 10.0 } });
            var pedido = new List<Pedido> { new Pedido(DateTime.Now, null, 1, "123", 10.0, true, new List<Produto> { produto }, null) };

            var fila = new List<PedidoStatus> { new PedidoStatus(pedido[0].Id.ToString(), "EmPreparacao", DateTime.Now) };

            _pedidoGatewayMock.Setup(g => g.GetAll()).ReturnsAsync(pedido);

            _pedidoStatusGatewayMock.Setup(g => g.GetAll()).ReturnsAsync(fila);

            // Act
            var result = await _atendimentoObterUseCase.ConsultarFilaPedidos();

            // Assert
            Assert.NotNull(result.Data);
            Assert.True(result.IsSuccess);

            _pedidoGatewayMock.Verify(g => g.GetAll(), Times.Once);
            _pedidoStatusGatewayMock.Verify(g => g.GetAll(), Times.Once);
        }

        [Fact]
        public async Task ConsultarFilaPedidos_WhenPedidoOrFilaDoesNotExist_ReturnsError()
        {
            // Arrange
            var pedido = new List<Pedido>();
            var fila = new List<PedidoStatus>();

            _pedidoGatewayMock.Setup(g => g.GetAll()).ReturnsAsync(pedido);

            _pedidoStatusGatewayMock.Setup(g => g.GetAll()).ReturnsAsync(fila);

            // Act
            var result = await _atendimentoObterUseCase.ConsultarFilaPedidos();

            // Assert
            Assert.Null(result.Data);
            Assert.Contains("Pedidos não localizado", result.Errors[0].Message);
        }
    }

}
