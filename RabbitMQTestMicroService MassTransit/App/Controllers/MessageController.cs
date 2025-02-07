using App.DTOs.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class MessageController : BaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public MessageController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ValueEntered request)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:event-listener"));

            await endpoint.Send(request);

            return Ok(new { Message = "Mesaj uğurla göndərildi" });
        }
    }
}
