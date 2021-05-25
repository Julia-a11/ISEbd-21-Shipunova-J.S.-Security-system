using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecuritySystemBusinessLogic.BusinessLogics;
using SecuritySystemBusinessLogic.HelperModels;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemDatabaseImplement.Implements;
using System;

namespace SecuritySystemRestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            MailLogic.MailConfig(new MailConfig
            {
                SmtpClientHost = configuration["SmtpClientHost"],
                SmtpClientPort = Convert.ToInt32(configuration["SmtpClientPort"]),
                MailLogin = configuration["MailLogin"],
                MailPassword = configuration["MailPassword"],
            });
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IClientStorage, ClientStorage>();
            services.AddTransient<IOrderStorage, OrderStorage>();
            services.AddTransient<ISecureStorage, SecureStorage>();
            services.AddTransient<IComponentStorage, ComponentStorage>();
            services.AddTransient<IStoreHouseStorage, StoreHouseStorage>();
            services.AddTransient<IMessageInfoStorage, MessageInfoStorage>();
            services.AddTransient<OrderLogic>();
            services.AddTransient<ClientLogic>();
            services.AddTransient<SecureLogic>();
            services.AddTransient<ComponentLogic>();
            services.AddTransient<MailLogic>();
            services.AddTransient<StoreHouseLogic>();
            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
