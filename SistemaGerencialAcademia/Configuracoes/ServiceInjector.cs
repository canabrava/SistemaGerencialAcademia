using Microsoft.Extensions.DependencyInjection;
using SistemaGerencialAcademia.Data.Repositorios;
using SistemaGerencialAcademia.Dominio.Interfaces.Repositorios;
using SistemaGerencialAcademia.Dominio.Interfaces.Servicos;
using SistemaGerencialAcademia.Servicos.Servicos;

namespace SistemaGerencialAcademia.Configuracoes
{
    public static class ServiceInjector
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.ResolveServices();
            services.ResolveRepositories();

            return services;
        }

        private static IServiceCollection ResolveServices(this IServiceCollection services)
        {
            services.AddScoped<IClienteServico, ClienteServico>();
            services.AddScoped<IPagamentoServico, PagamentoServico>();
            services.AddScoped<IPeriodoDeFeriasServico, PeriodoDeFeriasServico>();

            return services;
        }

        private static IServiceCollection ResolveRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            services.AddScoped<IEnderecoRepositorio, EnderecoRepositorio>();
            services.AddScoped<IInformacoesPagamentoRepositorio, InformacoesPagamentoRepositorio>();
            services.AddScoped<IPeriodoDeFeriasRepositorio, PeriodoDeFeriasRepositorio>();

            return services;
        }
    }
}
