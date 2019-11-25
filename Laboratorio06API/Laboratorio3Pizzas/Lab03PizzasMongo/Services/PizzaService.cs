using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab03PizzasMongo.Models;
using MongoDB.Driver;

namespace Lab03PizzasMongo.Services
{
    public class PizzaService
    {
        private readonly IMongoCollection<Pizza> collection;

        public PizzaService(IPizzaDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            collection = database.GetCollection<Pizza>(settings.PizzaCollectionName);
        }
        //Obtener todas las pizzas
        public List<Pizza> GetAll()
        {
            return collection.Find(x => true).ToList();
        }
        //Obtener una pizza deacuerdo a su id
        public Pizza GetPizza(string id) => collection.Find<Pizza>(x => x.Id == id).FirstOrDefault();
        //Crear nueva pizza
        public Pizza Post(Pizza pizza)
        {
            collection.InsertOne(pizza);
            return pizza;
        }
        //Modificar una pizza deacuerdo al id
        public void Update(string id, Pizza pizzaMod) => collection.ReplaceOne(x => x.Id == pizzaMod.Id, pizzaMod);
        //Eliminar una pizza deacuerdo al id
        public void Remove(string id) => collection.DeleteOne(x => x.Id == id);
    }
}
