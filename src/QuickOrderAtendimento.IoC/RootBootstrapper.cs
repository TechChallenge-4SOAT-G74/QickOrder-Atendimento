using Microsoft.Extensions.DependencyInjection;
using QuickOrderAtendimento.Application.Events;
using QuickOrderAtendimento.Application.UseCases;
using QuickOrderAtendimento.Application.UseCases.Interfaces;
using QuickOrderAtendimento.Domain.Adapters;
using QuickOrderAtendimento.Infra.Gateway;
using QuickOrderAtendimento.Infra.Gateway.Core;
using QuickOrderAtendimento.Infra.Gateway.Gateway;
using QuickOrderAtendimento.Infra.MQ;
using System.Diagnostics.CodeAnalysis;

namespace QuickOrderAtendimento.IoC
{
    [ExcludeFromCodeCoverage]
    public static class RootBootstrapper
    {
        public static void BootstrapperRegisterServices(this IServiceCollection services)
        {
            var assemblyTypes = typeof(RootBootstrapper).Assembly.GetNoAbstractTypes();

            services.AddHostedService<RabbitMqSub>();
            services.AddSingleton(typeof(IRabbitMqPub<>), typeof(RabbitMqPub<>));
            services.AddSingleton<IProcessaEvento, ProcessaEvento>();

            //Repositories MongoDB
            services.AddSingleton<IMondoDBContext, MondoDBContext>();
            services.AddSingleton<IPedidoGateway, PedidoGateway>();
            services.AddScoped<IPedidoStatusGateway, PedidoStatusGateway>();

            //UseCases
            services.AddScoped<IAtendimentoAtualizarUseCase, AtendimentoAtualizarUseCase>();
            services.AddScoped<IAtendimentoExcluirUseCase, AtendimentoExcluirUseCase>();
            services.AddScoped<IAtendimentoObterUseCase, AtendimentoObterUseCase>();


        }
    }
}
