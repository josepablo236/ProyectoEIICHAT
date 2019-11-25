using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersAPI.Models;
using MongoDB.Driver;

namespace UsersAPI.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> collection;

        public UserService(IUserDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            collection = database.GetCollection<User>(settings.UserCollectionName);
        }

        public List<User> GetAll()
        {
            return collection.Find(x => true).ToList();
        }
        //Obtener un deacuerdo a su user
        public User GetUs(string password, string _user) => collection.Find<User>(x => x.Password == password && x.User_ == _user).FirstOrDefault();
        //Crear nuevo user
        public User Post(User user)
        {
            collection.InsertOne(user);
            return user;
        }
        //Modificar una pizza deacuerdo al id
        public void Update(string _user, User userMod) => collection.ReplaceOne(x => x.User_ == userMod.User_, userMod);
    }
}
