using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetcliWebApi.Dtos;
using DotnetcliWebApi.Entities;
using DotnetcliWebApi.Hubs;
using DotnetcliWebApi.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

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
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>()
                .ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddSwaggerGen(
                options =>
                {
                    var provider = services.BuildServiceProvider()
                                        .GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(
                            description.GroupName,
                            new Info()
                            {
                                Title = $"Sample API {description.ApiVersion}",
                                Version = description.ApiVersion.ToString()
                            });
                    }
                });

            services.AddApiVersioning(config =>
             {
                 config.ReportApiVersions = true;
                 config.AssumeDefaultVersionWhenUnspecified = true;
                 config.DefaultApiVersion = new ApiVersion(1, 0);
                 config.ApiVersionReader = new HeaderApiVersionReader("api-version");
             });
            services.AddSignalR();
            services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
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

            app.UseSwaggerUI(
                    options =>
                    {
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint(
                                $"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
                        }
                    });

            var foodRepository = app.ApplicationServices.GetRequiredService<IFoodRepository>();
            AddTestData(foodRepository);

            app.UseSignalR(routes =>
                       {
                           routes.MapHub<FoodHub>("foodhub");
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

        private static void AddTestData(IFoodRepository repository)
        {
            for (int i = 0; i < 5; i++)
            {
                FoodItem foodItem = new FoodItem();
                foodItem.Created = DateTime.Now;
                foodItem.Calories = 999;
                foodItem.Name = Guid.NewGuid().ToString();
                foodItem.Type = "Starter";
                repository.Add(foodItem);
            }
        }
    }
}
