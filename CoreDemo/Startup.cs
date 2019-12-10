using BFO;
using DAO;
using Entity;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.IO;


namespace CoreDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //注册log4net
            Repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(Repository, new FileInfo("log4net.config"));
        }

        public IConfiguration Configuration { get; }
        private ILoggerRepository Repository;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddMemoryCache();

            //容器注入日志处理类
            services.AddSingleton<ILog>(LogManager.GetLogger(Repository.Name, "决策服务日志"));
            services.InjectDAO();
            services.InjectBFO();
            //生命周期演示
            services.AddTransient<IOperationTransient, Entity.Operation>();
            services.AddScoped<IOperationScoped, Entity.Operation>();
            services.AddSingleton<IOperationSingleton, Entity.Operation>();
            services.AddTransient<OperationService, OperationService>();

            //Api版本控制
            AddApiVersioning(services);

            //Swagger配置
            AddSwaggerGen(services);
        }

        private void AddApiVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("api-version"), new QueryStringApiVersionReader("api-version"));
            }).AddMvcCore().AddVersionedApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'VVV";
            }); ;
        }

        private void AddSwaggerGen(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName,
                         new Info()
                         {
                             Title = $"CoreDemo接口文档 v{description.ApiVersion}",
                             Version = description.ApiVersion.ToString(),
                             Description = "RESTful API for CoreDemo",
                             Contact = new Contact { Name = "guest", Email = "xxx@qq.com", Url = "" }
                         }
                    );
                }

                //Set the comments path for the swagger json and ui.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;

                var xmlComments = new List<string>()
                {"CoreDemo.xml", "Entity.xml"};


                xmlComments.ForEach(o =>
                {
                    var xmlPath = Path.Combine(basePath, o);
                    options.IncludeXmlComments(xmlPath, true);
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            //Swagger配置
            UseSwagger(app);

            app.UseMvc();
        }

        private void UseSwagger(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    c.RoutePrefix = string.Empty;//根路径访问swagger
                }

            });
        }
    }
}
