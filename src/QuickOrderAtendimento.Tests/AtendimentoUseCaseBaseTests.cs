using Moq;
using QuickOrderAtendimento.Application.UseCases;
using QuickOrderAtendimento.Domain.Adapters;
using QuickOrderAtendimento.Domain.Entities;
using Xunit;

namespace QuickOrderAtendimento.Tests
{
    public class AtendimentoUseCaseBaseTests
    {
        private readonly AtendimentoUseCaseBase _atendimentoUseCaseBase;
        private readonly Mock<IPedidoStatusGateway> _pedidoStatusGatewayMock;

        public AtendimentoUseCaseBaseTests()
        {
            _pedidoStatusGatewayMock = new Mock<IPedidoStatusGateway>();
            _atendimentoUseCaseBase = new AtendimentoUseCaseBase(_pedidoStatusGatewayMock.Object);
        }


        [Fact]
        public async Task AlterarStatusPedido_WhenPedidoExists_ShouldUpdatePedidoAndReturnSuccessResult()
        {
            // Arrange
            var codigoPedido = "123";
            var status = "PagamentoAprovado";
            var pedidoStatus = new PedidoStatus(codigoPedido, status, DateTime.Now);

            _pedidoStatusGatewayMock.Setup(g => g.GetValue("CodigoPedido", codigoPedido)).ReturnsAsync(pedidoStatus);

            // Act
            var result = await _atendimentoUseCaseBase.AlterarStatusPedidoBase(codigoPedido, status);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task AlterarStatusPedido_WhenPedidoDoesNotExist_ShouldReturnErrorResult()
        {
            // Arrange
            var codigoPedido = "123";
            var status = "PagamentoAprovado";
            var pedidoStatus = new PedidoStatus(codigoPedido, status, DateTime.Now);

            _pedidoStatusGatewayMock.Setup(g => g.GetValue("CodigoPedido", codigoPedido)).ReturnsAsync(pedidoStatus);

            // Act
            var result = await _atendimentoUseCaseBase.AlterarStatusPedidoBase("321", status);


            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Pedido não localizado.", result.Errors[0].Message);
        }

        
    }
}
