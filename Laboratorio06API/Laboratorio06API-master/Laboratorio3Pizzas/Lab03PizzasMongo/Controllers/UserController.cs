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
                return CreatedAtRoute("userCreada", new { id = _user.Id }, _user);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Pizza/5
        [HttpPut("{id}")]
        //public IActionResult Put(string _user, User userMod)
        //{
        //    //var pizza = _userService.GetUs(_user);
        //    //if (pizza == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //    //_userService.Update(_user, userMod);
        //    //return Ok(userMod);
        //}

        // GET: api/Pizza
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _userService.GetAll();
        }

    }
}
