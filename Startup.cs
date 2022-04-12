using DataAccessLayer.Repositories;
using DataAccessLayer.UnitOfWorks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sabri_Logistics_Backend
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
            services.AddDbContext<SabriLogisticsContext>(options => options.UseSqlServer(Configuration["ConnectionString:SabriLogisticDB"]));

            services.AddCors();

            services.AddControllers().AddNewtonsoftJson(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ILoginRepository, LoginRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IGSTInfoRepository, GstInfoRepository>();
            services.AddTransient<ILrEntryRepository, LrEntryRepository>();
            services.AddTransient<ICodeEntryRepository, CodeEntryRepository>();
            #endregion
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // Shows UseCors with CorsPolicyBuilder.
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sabri Logistics Backend");
            });
        }
    }
}
