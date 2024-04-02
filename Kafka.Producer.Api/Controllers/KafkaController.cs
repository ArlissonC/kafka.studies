using Kafka.Producer.Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Kafka.Producer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaController : ControllerBase
    {
        [HttpPost("SendMessage")]
        public async Task<ActionResult> SendMessage([FromServices] ProducerService service, [FromBody] Message message)
        {
            await service.SendMessage(message);

            return Ok("Mensagem enviada com sucesso!");
        }
    }
}
