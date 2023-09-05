using Microsoft.EntityFrameworkCore;
using Orama.AccountManager.Application.Queries.BankAccounts;
using Orama.AccountManager.Infrastructure.DB;
using Orama.AccountManager.Infrastructure.Repositories;
using Orama.AccountManager.Model.Repositories;
using Serilog.Events;
using Serilog.Filters;
using Serilog;
using Serilog.Exceptions;
using FluentValidation;
using Orama.AccountManager.Api.Validators;
using Orama.AccountManager.Api.Middleware;

namespace Orama.AccountManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithCorrelationId()
            .Enrich.WithProperty("Orama.AccountManager", $"API Serilog - {Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}")
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
            .Filter.ByExcluding(z => z.MessageTemplate.Text.Contains("Business error"))
            .WriteTo.Async(wt => wt.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"))
            .CreateLogger();

            builder.Host.UseSerilog(Log.Logger);

            #region Services
            builder.Services.AddMediatR(cfg => 
            { 
                cfg.RegisterServicesFromAssembly(typeof(GetAccountBalanceQuery).Assembly); 
            });

            builder.Services.AddValidatorsFromAssemblyContaining<CreateTransactionCommandValidator>();

            builder.Services
                .AddDbContext<AccountManagerDbContext>(x => 
                    x.UseSqlServer($"Data Source={Environment.GetEnvironmentVariable("ACCOUNTMANAGER_DBSERVER")};Initial Catalog={Environment.GetEnvironmentVariable("ACCOUNTMANAGER_DBNAME")};User Id={Environment.GetEnvironmentVariable("ACCOUNTMANAGER_DBUSER")};Password={Environment.GetEnvironmentVariable("ACCOUNTMANAGER_DBPASSWORD")};multipleactiveresultsets=True;application name=OramaAccountManager;TrustServerCertificate=true;"));

            builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            builder.Services.AddScoped<ITransactionRepository,  TransactionRepository>();
            builder.Services.AddScoped<IAssetRepository, AssetRepository>();
            #endregion

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            Migrate(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseAuthenticationMiddleware();

            app.MapControllers();

            app.Run();
        }

        static void Migrate(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AccountManagerDbContext>();
            db.Database.Migrate();
        }
    }
}