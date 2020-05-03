using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MySql.Core;
using SwaggerDemo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SwaggerDemo.Middleware;

namespace SwaggerDemo
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// 
        /// </summary>
        public IWebHostEnvironment _env { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var mySqlConnString = Configuration.GetConnectionString("MySqlConnString");

            services.AddControllers(
                opt =>
                {
                    //opt.ReturnHttpNotAcceptable = true;
                })
                .AddXmlSerializerFormatters()
                .AddXmlDataContractSerializerFormatters();//接口添加xml支持 xml区分大小写

            #region 注册SwaggerUI
            services.AddSwaggerRegister();
            #endregion

            #region 注册Hangfire
            services.AddHangfireRegister(mySqlConnString);
            #endregion

            #region 注册接口服务
            services.AddInterfaceServicesRegister(_env);
            #endregion
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region 启动Swagger
            app.UseSwaggerExtensions();
            #endregion

            #region Hangfire
            app.UseHangfireExtensions();
            #endregion

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
