using App.DTOs.Consumers;
using MassTransit;
using MassTransit.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;


namespace App.Controllers
{
    public class ValueController : BaseController
    {
        readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<ValueEntered> _requestClient;

        public ValueController(IPublishEndpoint publishEndpoint, IRequestClient<ValueEntered> requestClient)
        {
            _publishEndpoint = publishEndpoint;
            _requestClient = requestClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string value)
        {
            await _publishEndpoint.Publish<ValueEntered>(new
            {
                Value = value
            });

            return Ok();
        }

        //[HttpGet]
        //public async Task<IActionResult> GetMessage()
        //{
        //    var connectionFactory = new ConnectionFactory
        //    {
        //        HostName = "localhost",
        //        UserName = "guest",
        //        Password = "guest"
        //    };

        //    using var connection = connectionFactory.CreateConnection();
        //    using var channel = connection.CreateModel();

        //    var result = channel.BasicGet("event-listener", true);
        //    if (result == null)
        //    {
        //        return NotFound("No messages");
        //    }

        //    var message = Encoding.UTF8.GetString(result.Body.ToArray());
        //    return Ok(new { Message = message });
        //}

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ValueEntered request)
        {
            var response = await _requestClient.GetResponse<ValueEnteredResponse>(request);

            return Ok(new
            {
                ReceivedValue = response.Message.ReceivedValue,
                ProcessedAt = response.Message.ProcessedAt
            });
        }

    }
}
