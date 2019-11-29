using System.Collections.Generic;
using UsersAPI.Models;
using UsersAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;

        public MessageController(MessageService service)
        {
            _messageService = service;
        }

        // GET: api/Message/5
        [HttpGet("{messageVariables}", Name = "GetMessage")]
        public ActionResult<User> GetMessage (string messageVariables)
        {
            var variables = messageVariables.Split('|');
            var emisor = variables[0];
            var receptor = variables[1];
            var messageList = _messageService.GetMessage(emisor, receptor);
            if (messageList.Count() == 0)
            {
                return NotFound();
            }
            return Ok(messageList);
        }

        // POST: api/Message
        [HttpPost]
        public ActionResult<Message> Post(Message message)
        {
            if (ModelState.IsValid)
            {
                _messageService.PostMessage(message);
                return StatusCode(201);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public IEnumerable<Message> Get()
        {
            return _messageService.GetAll();
        }
    }
}