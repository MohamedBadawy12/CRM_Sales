using CRM_Sales_Application.Interfaces;
using CRM_Sales_Core.Interfaces;
using CRM_Sales_Infrastructure.Data;
using CRM_Sales_Infrastructure.ExportServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CRM_Sales_Infrastructure.Persistence.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IClientExportService, ClientExportService>();

            return services;
        }
    }
}
