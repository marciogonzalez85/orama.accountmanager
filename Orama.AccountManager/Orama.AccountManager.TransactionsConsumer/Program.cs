// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orama.AccountManager.Infrastructure.DB;
using Orama.AccountManager.Infrastructure.Repositories;
using Orama.AccountManager.Model.Repositories;
using Orama.AccountManager.TransactionsConsumer.RMQ;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Exceptions;
using System.Reflection.PortableExecutable;

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

var serviceCollection = new ServiceCollection();
ConfigureService(serviceCollection);
var serviceProvider = serviceCollection.BuildServiceProvider();

var consumer = serviceProvider.GetService<Consumer>();
consumer.Start();


static void ConfigureService(IServiceCollection services)
{
    services.AddScoped<IBankAccountRepository, BankAccountRepository>();
    services.AddScoped<ITransactionRepository, TransactionRepository>();
    services.AddScoped<IAssetRepository, AssetRepository>();
    services.AddSingleton<Consumer>();
    services.AddDbContext<AccountManagerDbContext>(x => x.UseSqlServer($"Data Source={Environment.GetEnvironmentVariable("ACCOUNTMANAGER_DBSERVER")};Initial Catalog={Environment.GetEnvironmentVariable("ACCOUNTMANAGER_DBNAME")};User Id={Environment.GetEnvironmentVariable("ACCOUNTMANAGER_DBUSER")};Password={Environment.GetEnvironmentVariable("ACCOUNTMANAGER_DBPASSWORD")};multipleactiveresultsets=True;application name=OramaAccountManager;TrustServerCertificate=true;"));

}
