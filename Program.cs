using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PortfolioManagement.Models;
using PortfolioManagement.Services;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "Portfolio_";
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<GestaoProdutosService>();
builder.Services.AddSingleton<TransacaoService>();

// Adicionar serviços simulados de produtos e clientes para exemplo
var produtos = new List<ProdutoFinanceiro>
{
    new ProdutoFinanceiro { Id = 1, Nome = "Produto 1", DataVencimento = DateTime.Now.AddDays(5), Valor = 1000m },
    new ProdutoFinanceiro { Id = 2, Nome = "Produto 2", DataVencimento = DateTime.Now.AddDays(10), Valor = 2000m },
};
builder.Services.AddSingleton(produtos);

builder.Services.AddSingleton<EmailService>();

// Configurar Quartz
builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
builder.Services.AddSingleton<NotificarProdutosJob>();
builder.Services.AddSingleton(new JobSchedule(
    jobType: typeof(NotificarProdutosJob),
    cronExpression: "0 0 8 * * ?")); // Executar todos os dias às 8:00

builder.Services.AddHostedService<QuartzHostedService>();

builder.Logging.AddConsole();
builder.Logging.AddDebug();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio API V1");
});

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

public class SingletonJobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SingletonJobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
    }

    public void ReturnJob(IJob job) { }
}

public class JobSchedule
{
    public Type JobType { get; }
    public string CronExpression { get; }

    public JobSchedule(Type jobType, string cronExpression)
    {
        JobType = jobType;
        CronExpression = cronExpression;
    }
}

public class QuartzHostedService : IHostedService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IJobFactory _jobFactory;
    private readonly IEnumerable<JobSchedule> _jobSchedules;
    private IScheduler _scheduler;

    public QuartzHostedService(
        ISchedulerFactory schedulerFactory,
        IJobFactory jobFactory,
        IEnumerable<JobSchedule> jobSchedules)
    {
        _schedulerFactory = schedulerFactory;
        _jobFactory = jobFactory;
        _jobSchedules = jobSchedules;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        _scheduler.JobFactory = _jobFactory;

        foreach (var jobSchedule in _jobSchedules)
        {
            var job = CreateJob(jobSchedule);
            var trigger = CreateTrigger(jobSchedule);

            await _scheduler.ScheduleJob(job, trigger, cancellationToken);
        }

        await _scheduler.Start(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _scheduler?.Shutdown(cancellationToken);
    }

    private static IJobDetail CreateJob(JobSchedule schedule)
    {
        var jobType = schedule.JobType;
        return JobBuilder
            .Create(jobType)
            .WithIdentity(jobType.FullName)
            .WithDescription(jobType.Name)
            .Build();
    }

    private static ITrigger CreateTrigger(JobSchedule schedule)
    {
        return TriggerBuilder
            .Create()
            .WithIdentity($"{schedule.JobType.FullName}.trigger")
            .WithCronSchedule(schedule.CronExpression)
            .WithDescription(schedule.CronExpression)
            .Build();
    }
}
