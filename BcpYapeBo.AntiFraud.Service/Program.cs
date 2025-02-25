using BcpYapeBo.AntiFraud.Application;
using BcpYapeBo.AntiFraud.Application.Ports.Driven;
using BcpYapeBo.AntiFraud.Application.Ports.Driving;
using BcpYapeBo.AntiFraud.Application.Services;
using BcpYapeBo.AntiFraud.Infrastructure;
using BcpYapeBo.AntiFraud.Infrastructure.Messaging;
using BcpYapeBo.AntiFraud.Infrastructure.Persistence;
using BcpYapeBo.AntiFraud.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<AntiFraudDbContext>(options =>
            options.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<ITransactionAntiFraudRepository, TransactionAntiFraudRepository>();
        services.AddScoped<IAntiFraudValidationPublisher, AntiFraudValidationPublisher>();
        services.AddScoped<ITransactionAntiFraudService, TransactionAntiFraudService>();
        
        services.AddHostedService<AntiFraudTransactionConsumer>();
    });

var host = builder.Build();
await host.RunAsync();