using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SwaggerDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerDemo.Middleware
{
    /// <summary>
    /// 接口服务注册
    /// </summary>
    public static class InterfaceServicesMiddleware
    {
        private static IWebHostEnvironment env { get; }

        /// <summary>
        /// 接口服务注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        public static void AddInterfaceServicesRegister(this IServiceCollection services, IWebHostEnvironment env)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddTransient<IHangfireServices, HangfireServices>();
        }
    }
}
