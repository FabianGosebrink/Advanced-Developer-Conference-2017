using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetcliWebApi.Dtos;
using DotnetcliWebApi.Entities;
using DotnetcliWebApi.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace DotnetcliWebApi
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
            services.AddOptions();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddSingleton<IFoodRepository, FoodRepository>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "API v1"
                });

                options.SwaggerDoc("v2", new Info
                {
                    Version = "v2",
                    Title = "API v2"
                });
            });

            services.AddApiVersioning(config =>
             {
                 config.ReportApiVersions = true;
                 config.AssumeDefaultVersionWhenUnspecified = true;
                 config.DefaultApiVersion = new ApiVersion(1, 0);
                 config.ApiVersionReader = new HeaderApiVersionReader("api-version");
             });

            services.AddMvcCore()
                .AddJsonFormatters(options =>
                    options.ContractResolver = new CamelCasePropertyNamesContractResolver())
                .AddApiExplorer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/plain";
                        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (errorFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500, errorFeature.Error, errorFeature.Error.Message);
                        }

                        await context.Response.WriteAsync("There was an error");
                    });
                });
            }

            app.UseSwagger();

            app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    options.SwaggerEndpoint("/swagger/v2/swagger.json", "API v2");
                });

            app.UseCors("AllowAllOrigins");
            AutoMapper.Mapper.Initialize(mapper =>
                      {
                          mapper.CreateMap<FoodItem, FoodItemDto>().ReverseMap();
                          mapper.CreateMap<FoodItem, FoodUpdateDto>().ReverseMap();
                          mapper.CreateMap<FoodItem, FoodCreateDto>().ReverseMap();
                      });
            app.UseMvc();
        }
    }
}
