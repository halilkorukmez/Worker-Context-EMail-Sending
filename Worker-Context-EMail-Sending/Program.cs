using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SendGrid;
using Worker_Context_EMail_Sending.WorkerService;

namespace Worker_Context_EMail_Sending
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config => config.AddUserSecrets(Assembly.GetExecutingAssembly()))
                .ConfigureServices(( services) =>
                {
                    var sendGridApiKey = "YOUR API KEY";

                    if (string.IsNullOrWhiteSpace(sendGridApiKey))
                        throw new InvalidOperationException("Enter a valid SendGrid API key!");

                    services.AddScoped<ISendGridClient>(s => new SendGridClient(new SendGridClientOptions
                    {
                        ApiKey = sendGridApiKey
                    }));

                    services.AddHostedService<Worker>();
                });
    }
}