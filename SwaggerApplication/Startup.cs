using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;

namespace SwaggerApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddApiVersioning(options =>
            {
                options.UseApiBehavior = false;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                        new HeaderApiVersionReader("x-api-version"),
                        new QueryStringApiVersionReader(),
                        new UrlSegmentApiVersionReader());
            });

            services.AddVersionedApiExplorer(p =>
               {
                   p.GroupNameFormat = "'v'V";
                   p.SubstituteApiVersionInUrl = true;
               });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });


            services.AddSwaggerGen(c =>
            {
                var provider = services.BuildServiceProvider()
               .GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = "TESTE e o titulo do Swagger",
                        Version = description.ApiVersion.ToString(),
                        Contact = new OpenApiContact
                        {
                            Name = "Ibrascan",
                            Email = "larissa.martins@ibrascan.com"
                        },
                        Description = "Essa é uma descrição mais detalhada referente a documentação do Sistema."
                    });
                }

                var path = Path.Combine(AppContext.BaseDirectory, "SwaggerApplication.xml");
                c.IncludeXmlComments(path);
            });

            services.AddCors(options =>
             {
                 options.AddPolicy(MyAllowSpecificOrigins,
                 builder =>
                 {
                     builder.AllowAnyOrigin()
                             .AllowAnyHeader()
                            .AllowAnyMethod();
                 });
             });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c => 
            {
                c.DocumentTitle = "Título da Aba";


                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
                }

                c.RoutePrefix = string.Empty;
                c.DocExpansion(DocExpansion.List);
            });
             
            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();
            app.UseEndpoints(n =>
            {
                n.MapControllers();
            });
        }
    }
}
