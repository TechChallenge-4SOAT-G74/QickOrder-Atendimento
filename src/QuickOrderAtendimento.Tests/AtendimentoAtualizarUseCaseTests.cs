using Moq;
using QuickOrderAtendimento.Application.UseCases;
using QuickOrderAtendimento.Domain.Adapters;
using QuickOrderAtendimento.Domain.Entities;
using QuickOrderAtendimento.Infra.MQ;
using Xunit;

namespace QuickOrderAtendimento.Tests
{

    public class AtendimentoAtualizarUseCaseTests
    {
        private readonly AtendimentoAtualizarUseCase _atendimentoAtualizarUseCase;
        private readonly Mock<IPedidoGateway> _pedidoGatewayMock;
        private readonly Mock<IPedidoStatusGateway> _pedidoStatusGatewayMock;
        private readonly Mock<IRabbitMqPub<Pedido>> _rabbitMqPubMock;

        public AtendimentoAtualizarUseCaseTests()
        {
            _pedidoGatewayMock = new Mock<IPedidoGateway>();
            _pedidoStatusGatewayMock = new Mock<IPedidoStatusGateway>();
            _rabbitMqPubMock = new Mock<IRabbitMqPub<Pedido>>();
            _atendimentoAtualizarUseCase = new AtendimentoAtualizarUseCase(_pedidoStatusGatewayMock.Object, _pedidoGatewayMock.Object, _rabbitMqPubMock.Object);
        }


        [Fact]
        public async Task AlterarStatusPedido_WhenPedidoExists_ShouldUpdatePedidoAndReturnSuccessResult()
        {
            // Arrange
            var codigoPedido = "123";
            var pedidoStatus = "Pagamento Aprovado";
            var produto = new Produto("Lanche", "Produto 1", 1, 2, 10.0,
                new List<ProdutoItens> { new ProdutoItens { NomeProdutoItem = "Pão", Quantidade = 1, ValorItem = 10.0 } });
            var pedido = new Pedido(DateTime.Now, null, 1, "123", 10.0, true, new List<Produto> { produto }, null);

            _pedidoGatewayMock.Setup(g => g.Get(pedido.Id.ToString())).ReturnsAsync(pedido);

            // Act
            var result = await _atendimentoAtualizarUseCase.AlterarStatusPedido(pedido.Id.ToString(), pedidoStatus);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(pedido.PedidoPago);
        }

        [Fact]
        public async Task AlterarStatusPedidoo_WhenPedidoDoesNotExist_ShouldReturnErrorResult()
        {
            // Arrange
            var codigoPedido = "123";
            var pedidoStatus = "Pago";
            var produto = new Produto("Lanche", "Produto 1", 1, 2, 10.0,
                new List<ProdutoItens> { new ProdutoItens { NomeProdutoItem = "Pão", Quantidade = 1, ValorItem = 10.0 } });
            var pedido = new Pedido(DateTime.Now, null, 1, "321", 10.0, false, new List<Produto> { produto }, null);

            _pedidoGatewayMock.Setup(g => g.Get(pedido.Id.ToString())).ReturnsAsync(pedido);

            // Act
            var result = await _atendimentoAtualizarUseCase.AlterarStatusPedido(codigoPedido, pedidoStatus);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Pedido não localizado.", result.Errors[0].Message);
        }
    }
}
