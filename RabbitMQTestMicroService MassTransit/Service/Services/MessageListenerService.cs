//using MassTransit;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Service.Services
//{
//    public class MessageListenerService : BackgroundService
//    {
//        private readonly ILogger<MessageListenerService> _logger;
//        private readonly IBusControl _busControl;

//        public MessageListenerService(ILogger<MessageListenerService> logger, IBusControl busControl)
//        {
//            _logger = logger;
//            _busControl = busControl;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            _logger.LogInformation("Message listener service is starting.");

//            await _busControl.StartAsync(stoppingToken);

//            _logger.LogInformation("Message listener service is running and listening for messages.");

//        }

//        public override async Task StopAsync(CancellationToken stoppingToken)
//        {
//            _logger.LogInformation("Message listener service is stopping.");

//            await _busControl.StopAsync(stoppingToken);

//            await base.StopAsync(stoppingToken);
//        }
//    }
//}
