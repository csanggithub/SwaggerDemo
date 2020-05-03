using Hangfire;
using Hangfire.MySql.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SwaggerDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerDemo.Middleware
{
    /// <summary>
    /// Hangfire中间件
    /// </summary>
    public static class HangfireMiddleware
    {
        /// <summary>
        /// Hangfire注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="mySqlConnStr"></param>
        public static void AddHangfireRegister(this IServiceCollection services, string mySqlConnStr)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddHangfire(x => x.UseStorage(new MySqlStorage(mySqlConnStr, new MySqlStorageOptions() { TablePrefix = "hf_" })));
        }

        /// <summary>
        /// Hangfire启动
        /// </summary>
        /// <param name="app"></param>
        public static void UseHangfireExtensions(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                WorkerCount = 1 //允许进程链接数量 
            });
            app.UseHangfireDashboard("/hfire", new DashboardOptions
            {
                DisplayStorageConnectionString = false,
                AppPath = "/" //返回的url
            });
            RecurringJob.AddOrUpdate<IHangfireServices>(x => x.TaskOne(), "0/10 * * * *");
            RecurringJob.AddOrUpdate<IHangfireServices>(x => x.TaskTwo(), Cron.Minutely);
        }
    }
}
