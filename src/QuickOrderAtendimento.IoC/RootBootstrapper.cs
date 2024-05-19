using Microsoft.Extensions.DependencyInjection;
using QuickOrderAtendimento.Application.UseCases;
using QuickOrderAtendimento.Application.UseCases.Interfaces;
using QuickOrderAtendimento.Domain.Adapters;
using QuickOrderAtendimento.Infra.Gateway;
using QuickOrderAtendimento.Infra.Gateway.Core;
using QuickOrderAtendimento.Infra.Gateway.Gateway;
using QuickOrderAtendimento.Infra.MQ;

namespace QuickOrderAtendimento.IoC
{
    public static class RootBootstrapper
    {
        public static void BootstrapperRegisterServices(this IServiceCollection services)
        {
            var assemblyTypes = typeof(RootBootstrapper).Assembly.GetNoAbstractTypes();

            services.AddSingleton(typeof(IRabbitMqPub<>), typeof(RabbitMqPub<>));
            services.AddScoped<IProcessaEvento, ProcessaEvento>();

            //Repositories MongoDB
            services.AddSingleton<IMondoDBContext, MondoDBContext>();
            services.AddScoped<ICarrinhoGateway, CarrinhoGateway>();
            services.AddScoped<IPedidoGateway, PedidoGateway>();
            services.AddScoped<IPedidoStatusGateway, PedidoStatusGateway>();

            //UseCases
            services.AddScoped<IPedidoAtualizarUseCase, PedidoAtualizarUseCase>();
            services.AddScoped<IPedidoExcluirUseCase, PedidoExcluirUseCase>();
            services.AddScoped<IPedidoCriarUseCase, PedidoCriarUseCase>();
            services.AddScoped<IPedidoObterUseCase, PedidoObterUseCase>();


        }
    }
}
