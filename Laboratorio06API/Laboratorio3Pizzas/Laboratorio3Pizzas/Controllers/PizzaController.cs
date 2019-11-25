using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laboratorio3Pizzas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laboratorio3Pizzas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public PizzaController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Pizza> Get()
        {
            return context.Pizzas.ToList();
        }
        [HttpGet("{id}", Name = "pizzaCreada")]
        public ActionResult GetByID(int id)
        {
            var pizza = context.Pizzas.FirstOrDefault(x => x.Id == id);

            if(pizza == null)
            {
                return NotFound();
            }
            return Ok(pizza);
        }
        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] Pizza pizza)
        {
            if(ModelState.IsValid)
            {
                context.Pizzas.Add(pizza);
                context.SaveChanges();
                return new CreatedAtRouteResult("pizzaCreada", new { id = pizza.Id }, pizza);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] Pizza pizza, int id)
        {
            if(pizza.Id != id)
            {
                return BadRequest();
            }
            context.Entry(pizza).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var pizza = context.Pizzas.FirstOrDefault(x => x.Id == id);
            if(pizza == null)
            {
                return NotFound();
            }
            context.Pizzas.Remove(pizza);
            context.SaveChanges();
            return Ok(pizza);
        }
    }
}