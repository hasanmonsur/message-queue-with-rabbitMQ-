using MessageQueueApp.Contacts;
using MessageQueueApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessageQueueApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageQueueService _queueService;
        private readonly IMessageRepository _repository;

        public MessagesController(IMessageQueueService queueService, IMessageRepository repository)
        {
            _queueService = queueService;
            _repository = repository;
        }

        [HttpPost("send")]
        public IActionResult SendMessage([FromBody] string message)
        {
            _queueService.PublishMessage(message);
            return Ok("Message sent to the queue.");
        }

        [HttpGet("consume")]
        public IActionResult ConsumeMessages()
        {
            _queueService.ConsumeMessages();
            return Ok("Started consuming messages.");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _repository.GetMessagesAsync();
            return Ok(messages);
        }
    }
}
