using MassTransit;

namespace App.DTOs.Consumers
{

    class ValueEnteredEventConsumer : IConsumer<ValueEntered>
    {
        private readonly ILogger<ValueEnteredEventConsumer> _logger;

        public ValueEnteredEventConsumer(ILogger<ValueEnteredEventConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ValueEntered> context)
        {

            var value = context.Message.Value;

            _logger.LogInformation("Value: {Value}", context.Message.Value);

            await context.RespondAsync(new ValueEnteredResponse
            {
                ReceivedValue = value,
                ProcessedAt = DateTime.UtcNow
            });
            
            //My Logical In Hire
            await Task.CompletedTask;
        }
    }
}
