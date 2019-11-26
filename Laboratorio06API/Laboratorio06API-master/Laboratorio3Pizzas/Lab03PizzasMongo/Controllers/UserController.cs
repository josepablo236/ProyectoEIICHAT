using System.Collections.Generic;
using UsersAPI.Models;
using UsersAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService service)
        {
            _userService = service;
        }

        // GET: api/User/5
        [HttpGet("{userVariables}", Name = "Get")]
        public ActionResult<User> Get(string userVariables)
        {
            var variables = userVariables.Split('|');
            var username = variables[0];
            var pass = variables[1];
            var user = _userService.GetUs(pass, username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/Pizza
        [HttpPost]
        public ActionResult<User> Post(User _user)
        {
            if (ModelState.IsValid)
            {
                _userService.Post(_user);
                return StatusCode(201);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Pizza/5
        [HttpPut]
        public IActionResult Put(User userMod)
        {
            var user = _userService.GetUser(userMod.User_);
            if (user == null)
            {
                return NotFound();
            }
            userMod.Id = user.Id;
            _userService.Update(user.Id, userMod);
            return Ok(user);
        }

        // GET: api/Pizza
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _userService.GetAll();
        }

    }
}
