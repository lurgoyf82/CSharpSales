
using CSharpSales.Handlers;
using CSharpSales.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace CSharpSales
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // application services
            builder.Services.AddScoped<InputParser>();
            builder.Services.AddScoped<CartOutputFormatter>();
            builder.Services.AddScoped<CartService>();
            builder.Services.AddScoped<GetCartResponseHandler>();


            // Register MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            // Register Swagger/OpenAPI explicitly
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CSharpSales API",
                    Version = "v1"
                });
                options.ExampleFilters();
            });
            builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();


            //// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "CSharpSales API v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
