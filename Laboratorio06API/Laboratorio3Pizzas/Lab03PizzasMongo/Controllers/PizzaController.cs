using System.Collections.Generic;
using Lab03PizzasMongo.Models;
using Lab03PizzasMongo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab03PizzasMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly PizzaService _pizzaService;

        public PizzaController(PizzaService service)
        {
            _pizzaService = service;
        }
        // GET: api/Pizza
        [HttpGet]
        public IEnumerable<Pizza> Get()
        {
            return _pizzaService.GetAll();
        }

        // GET: api/Pizza/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Pizza> Get(string id)
        {
            var pizza = _pizzaService.GetPizza(id);
            if(pizza == null)
            {
                return NotFound();
            }
            return Ok(pizza);
        }

        // POST: api/Pizza
        [HttpPost]
        public ActionResult<Pizza> Post(Pizza pizza)
        {
            if (ModelState.IsValid)
            {
                _pizzaService.Post(pizza);
                return CreatedAtRoute("pizzaCreada", new { id = pizza.Id }, pizza);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Pizza/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, Pizza pizzaMod)
        {
            var pizza = _pizzaService.GetPizza(id);
            if (pizza == null)
            {
                return NotFound();
            }
            _pizzaService.Update(id, pizzaMod);
            return Ok(pizzaMod);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult<Pizza> Delete(string id)
        {
            var pizza = _pizzaService.GetPizza(id);
            if (pizza == null)
            {
                return NotFound();
            }
            _pizzaService.Remove(id);
            return Ok(pizza);
        }
    }
}
