using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Register.Api
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "API de Registro Usuário",
                    Version = "1",
                    Description = "API que fornece uma interface comum para a comunicação do aplicativo móvel para registro dos usuáriosl"
                });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Entre com o token para efetuar consultas, exemplo: bearer token",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });


                options.AddSecurityRequirement(security);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerUIConfig(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zona Azul API");
            });

            return app;
        }
    }

}


