using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectorySettlementsBLL.Infrastructure;
using DirectorySettlementsDAL.Data;
using DirectorySettlementsDAL.Entities;
using DirectorySettlementsDAL.Interfaces;
using DirectorySettlementsDAL.Repositories;
using DirectorySettlementsWebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DirectorySettlementsWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.Bind("ConfigData", new Config());

            //services.AddTransient<IRepository<Settlement>, SettlementRepository>();
            //services.AddTransient<IUnitOfWork, EFUnitOfWork>();

            //services.AddDbContext<ApplicationContext>(x => x.UseSqlServer(Config.DbConnection));
            services.AddDal(Config.DbConnection);
            services.AddBll();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
