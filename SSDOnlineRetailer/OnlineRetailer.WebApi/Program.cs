
using Microsoft.EntityFrameworkCore;
using OnlineRetailer.Core;
using OnlineRetailer.Core.Entities;
using OnlineRetailer.Core.Interfaces;
using OnlineRetailer.Core.Services;
using OnlineRetailer.CredentialsHandler;
using OnlineRetailer.Infrastructure;
using OnlineRetailer.Infrastructure.Repositories;
using Prometheus;
namespace OnlineRetailer.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<OnlineRetailerContext>(opt =>
                opt.UseInMemoryDatabase("OnlineRetailerDb"));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IRepository<Product>, ProductRepository>();
            builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
            builder.Services.AddScoped<IRepository<Order>, OrderRepository>();
            builder.Services.AddScoped<IOrderManager, OrderManager>();
            builder.Services.AddTransient<IDbInitializer, DbInitializer>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<LoginThrottler>(provider =>
            {
                var maxAttempts = 5;
                var lockoutPeriod = TimeSpan.FromMinutes(5);

                return new LoginThrottler(maxAttempts, lockoutPeriod);
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var dbContext = services.GetService<OnlineRetailerContext>();
                    var dbInitializer = services.GetService<IDbInitializer>();
                    dbInitializer.Initialize(dbContext);
                }
            app.UseHttpMetrics();

            app.UseAuthorization();

            app.MapControllers();

            app.MapMetrics();

            app.Run("http://0.0.0.0:80");
        }
    }
}
