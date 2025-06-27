using CSharpSales.Handlers;
using CSharpSales.Services;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace CSharpSales
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager configuration = builder.Configuration;

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


            // CORS configuration
            bool allowAll = configuration.GetValue<bool>("Client:AllowAll");

            builder.Services.AddCors(cors =>
            {
                cors.AddPolicy("AllowOrigins", options =>
                {

                    options.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();


                    //if (allowAll)
                    //{
                    //    options.AllowAnyOrigin()
                    //           .AllowAnyMethod()
                    //           .AllowAnyHeader();
                    //}
                    //else
                    //{
                    //    string[] allowedEndpoints = configuration
                    //        .GetSection("Client:Endpoints")
                    //        .GetChildren()
                    //        .Select(x => x.Value ?? string.Empty)
                    //        .ToArray();

                    //    options.WithOrigins(allowedEndpoints)
                    //           .AllowAnyMethod()
                    //           .AllowAnyHeader()
                    //           .AllowCredentials();
                    //}
                });
            });


            //// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() ||
                configuration.GetValue<bool>("EnableSwagger"))
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "CSharpSales API v1");
                });
            }


            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AllowOrigins");

            app.MapControllers();

            app.Run();
        }
    }
}
