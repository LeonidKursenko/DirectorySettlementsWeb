using DirectorySettlementsBLL.BusinessModels;
using DirectorySettlementsBLL.Interfaces;
using DirectorySettlementsBLL.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorySettlementsWebApi.Services
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Sets Business logic layer dependency injections.
        /// </summary>
        /// <param name="services"></param>
        public static void AddBll(this IServiceCollection services)
        {
            services.AddTransient<IFilterOptions, FilterOptions>();
            services.AddTransient<IDirectoryService, DirectoryService>();
            services.AddTransient<IExportService, ExportService>();
        }
    }
}
