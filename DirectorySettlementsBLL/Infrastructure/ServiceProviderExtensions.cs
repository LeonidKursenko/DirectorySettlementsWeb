using DirectorySettlementsDAL.Data;
using DirectorySettlementsDAL.Entities;
using DirectorySettlementsDAL.Interfaces;
using DirectorySettlementsDAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySettlementsBLL.Infrastructure
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Sets Data access layer dependency injections.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">Sets connection string to the database.</param>
        public static void AddDal(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IRepository<Settlement>, SettlementRepository>();
            services.AddTransient<IUnitOfWork, EFUnitOfWork>();

            services.AddDbContext<ApplicationContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
