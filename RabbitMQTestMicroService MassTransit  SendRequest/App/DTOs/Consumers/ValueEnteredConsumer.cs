using MassTransit;

namespace App.DTOs.Consumers
{
    public class ValueEnteredConsumer : IConsumer<ValueEntered>
    {
        private readonly ILogger<ValueEnteredConsumer> _logger;

        public ValueEnteredConsumer(ILogger<ValueEnteredConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ValueEntered> context)
        {
            var value = context.Message.Value;
                
            _logger.LogInformation("Received message: {Value}", value);

            await context.RespondAsync(new ValueEnteredResponse
            {
                Result = $"Processed: {value}"
            });

            //await Task.CompletedTask;
        }
    }
}
