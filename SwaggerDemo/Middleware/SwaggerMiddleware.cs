using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SwaggerDemo.Middleware
{
    /// <summary>
    /// 注册SwaggerUI
    /// </summary>
    public static class SwaggerMiddleware
    {
        /// <summary>
        /// 注册SwaggerUI
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerRegister(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // Register the Swagger generator, defining 1 or more Swagger documents
            // 注册Swagger生成器，定义一个或多个Swagger文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Web API",
                    Description = "Web API示例",
                    TermsOfService = new Uri("https://xxxxx.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Name",
                        Email = string.Empty,
                        Url = new Uri("https://xxxxx.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://xxxxx.com/license"),
                    }
                });
                c.SwaggerDoc("Test1", new OpenApiInfo
                {
                    Version = "Test1",
                    Title = "Test1 Web API",
                    Description = "Test1 Web API示例",
                });
                c.SwaggerDoc("Test2", new OpenApiInfo
                {
                    Version = "Test2",
                    Title = "Test2 Web API",
                    Description = "Test2 Web API示例",
                });

                //设置要展示的接口
                c.DocInclusionPredicate((docName, apiDes) =>
                {
                    if (!apiDes.TryGetMethodInfo(out MethodInfo method))
                        return false;
                    /*使用ApiExplorerSettingsAttribute里面的GroupName进行特性标识
                     * DeclaringType只能获取controller上的特性
                     * 我们这里是想以action的特性为主
                     * */
                    var version = method.DeclaringType.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
                    if (docName == "v1" && !version.Any())
                        return true;
                    //这里获取action的特性
                    var actionVersion = method.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
                    if (actionVersion.Any())
                        return actionVersion.Any(v => v == docName);
                    return version.Any(v => v == docName);
                });

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    new string[] { }
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                // 设置Swagger JSON和UI的注释路径。
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath,true);//加参数includeControllerXmlComments=true 显示控制器注释

                var basePath = ApplicationEnvironment.ApplicationBasePath;
                var xmlModelPath = Path.Combine(basePath, "Dtos.xml");//这个就是Model层的xml文件名
                c.IncludeXmlComments(xmlModelPath);
            });
        }

        /// <summary>
        /// 启动Swagger
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerExtensions(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            // 使中间件能够将生成的Swagger用作JSON端点。
            app.UseSwagger();

            // Enable middleware to serve swagger-ui(HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            // 使中间件能够为swagger ui（HTML、JS、CSS等）提供服务，
            // 指定Swagger JSON端点。
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.SwaggerEndpoint("/swagger/Test1/swagger.json", "My API Test1");
                c.SwaggerEndpoint("/swagger/Test2/swagger.json", "My API Test2");
                c.RoutePrefix = string.Empty;
                //c.DocExpansion(DocExpansion.List);//None折叠所有方法,Full展开所有方法
                //c.DefaultModelsExpandDepth(-1);//设置为-1 可不显示models
                //[ApiExplorerSettings(IgnoreApi = true)]//IgnoreApi = true 忽略某个Api
            });
        }
    }
}
