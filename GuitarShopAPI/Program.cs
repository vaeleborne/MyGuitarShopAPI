
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Data.Ado.Repositories;
using MyGuitarShop.Data.EFCore.Context;
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
                AddLogging(builder);

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();                     // serves /swagger/v1/swagger.json
                    app.UseSwaggerUI();                   // serves UI at /swagger

                    app.UseHttpsRedirection();
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

            builder.Services.AddSingleton(new SqlConnectionFactory(connection_string)); //ADO.NET Specific

            //ADO.NET Services
            builder.Services.AddScoped<ProductRepoADO>();
            builder.Services.AddScoped<AdministratorRepoADO>();
            builder.Services.AddScoped<CategoryRepoADO>();
            builder.Services.AddScoped<CustomerRepoADO>();
            builder.Services.AddScoped<AddressRepoADO>();
            builder.Services.AddScoped<OrderRepoADO>();

            builder.Services.AddDbContextFactory<MyGuitarShopContext>(options =>
            options.UseSqlServer(connection_string)); //EF CORE Specific


            //EFCORE Services
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.ProductRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.CategoryRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.AddressRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.CustomerRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.OrderRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.OrderItemRepository>();
            builder.Services.AddScoped<MyGuitarShop.Data.EFCore.Repositories.AdministratorRepository>();

       

            //MongoDB Specific
            var mongoConnectionString = builder.Configuration.GetConnectionString("MyGuitarShop_MongoDB")
                ?? throw new InvalidOperationException("MongoDb connection string not found.");

            builder.Services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(mongoConnectionString));

            builder.Services.AddSingleton<IMongoDatabase>(sp =>
            {
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                return mongoClient.GetDatabase("MyGuitarShop");
            });

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
