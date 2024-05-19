using Moq;
using QuickOrderAtendimento.Application.UseCases;
using QuickOrderAtendimento.Domain.Adapters;
using QuickOrderAtendimento.Domain.Entities;
using QuickOrderAtendimento.Infra.MQ;
using Xunit;

namespace QuickOrderAtendimento.Tests
{
    public class AtendimentoExcluirUseCaseTests
    {
        private readonly AtendimentoExcluirUseCase _atendimentoExcluirUseCase;
        private readonly Mock<IPedidoGateway> _pedidoGatewayMock;
        private readonly Mock<IPedidoStatusGateway> _pedidoStatusGatewayMock;
        private readonly Mock<IRabbitMqPub<Pedido>> _rabbitMqPubMock;

        public AtendimentoExcluirUseCaseTests()
        {
            _pedidoGatewayMock = new Mock<IPedidoGateway>();
            _pedidoStatusGatewayMock = new Mock<IPedidoStatusGateway>();
            _rabbitMqPubMock = new Mock<IRabbitMqPub<Pedido>>();
            _atendimentoExcluirUseCase = new AtendimentoExcluirUseCase(_pedidoStatusGatewayMock.Object, _pedidoGatewayMock.Object, _rabbitMqPubMock.Object);
        }

        [Fact]
        public async Task CancelarPedido_WhenPedidoExists_ShouldDeletePedidoAndCarrinho()
        {
            // Arrange
            var produto = new Produto("Lanche", "Produto 1", 1, 2, 10.0,
                new List<ProdutoItens> { new ProdutoItens { NomeProdutoItem = "Pão", Quantidade = 1, ValorItem = 10.0 } });

            var pedido = new Pedido(DateTime.Now, null, 1, "123", 10.0, true, new List<Produto> { produto }, null);

            _pedidoGatewayMock.Setup(g => g.Get(pedido.Id.ToString())).ReturnsAsync(pedido);

            // Act
            var result = await _atendimentoExcluirUseCase.CancelarPedido(pedido.Id.ToString(), "Pagamento não aprovado");

            // Assert
            Assert.True(result.IsSuccess);
            _pedidoGatewayMock.Verify(g => g.Update(pedido), Times.Once);
        }

        [Fact]
        public async Task CancelarPedido_WhenPedidoDoesNotExist_ShouldReturnError()
        {
            // Arrange
            var produto = new Produto("Lanche", "Produto 1", 1, 2, 10.0,
                 new List<ProdutoItens> { new ProdutoItens { NomeProdutoItem = "Pão", Quantidade = 1, ValorItem = 10.0 } });

            var pedido = new Pedido(DateTime.Now, null, 1, "123", 10.0, true, new List<Produto> { produto }, null);

            _pedidoGatewayMock.Setup(g => g.Get(pedido.Id.ToString())).ReturnsAsync((Pedido)null);

            // Act
            var result = await _atendimentoExcluirUseCase.CancelarPedido("123", "Pagamento não aprovado");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Pedido não encontrado.", result.Errors[0].Message);
        }
    }
}
