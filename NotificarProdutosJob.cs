using System.Threading.Tasks;
using Quartz;
using PortfolioManagement.Services;

public class NotificarProdutosJob : IJob
{
    private readonly EmailService _emailService;

    public NotificarProdutosJob(EmailService emailService)
    {
        _emailService = emailService;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _emailService.NotificarProdutosProximoVencimento();
        return Task.CompletedTask;
    }
}
