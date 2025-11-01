
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Data.SqlClient;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Data.Ado.Repositories;
using System;
using System.Data;
using System.Diagnostics;


namespace GuitarShopAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

                AddServices(builder);

                // Add services to the container.

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();                     // serves /swagger/v1/swagger.json
                    app.UseSwaggerUI();                   // serves UI at /swagger
                }

                ConfigureApplication(app);

                await app.RunAsync();
            }
            catch (Exception ex)
            {

                if(Debugger.IsAttached) Debugger.Break();

                Console.WriteLine(ex.ToString());
            }
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            var connection_string = builder.Configuration.GetConnectionString("MyGuitarShop")
                ?? throw new InvalidOperationException("MyGuitarShop connection string not found.");

            builder.Services.AddSingleton(new SqlConnectionFactory(connection_string));
            builder.Services.AddScoped<ProductRepo>();

            builder.Services.AddControllers();
        }

        private static void AddLogging(WebApplicationBuilder builder)
        {
            builder.Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging
                .AddFilter("Microsoft", LogLevel.Information)
                .AddFilter("Microsoft.AspNetCore.HttpLogging", LogLevel.Information)
                .AddConsole();
            });

            builder.Services.AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.RequestPath
                                        | HttpLoggingFields.RequestMethod
                                        | HttpLoggingFields.ResponseStatusCode;
            });
        }
        private static void ConfigureApplication(WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
        }
    }


}
