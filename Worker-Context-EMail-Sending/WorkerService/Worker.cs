using SendGrid;
using SendGrid.Helpers.Mail;

namespace Worker_Context_EMail_Sending.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    }
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
         
            using var scope = _serviceScopeFactory.CreateScope();

            var sendGridClient = scope.ServiceProvider.GetRequiredService<ISendGridClient>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var message = new SendGridMessage
            {
                Subject = "Info",
                PlainTextContent = "system is working !!",
                From = new EmailAddress(configuration["Email:From"]),

            };

            message.AddTo(configuration["Email:Recipient"]);
       

            await sendGridClient.SendEmailAsync(message, cancellationToken);

            await Task.Delay(TimeSpan.FromMinutes(30), cancellationToken);
        }
    }
}