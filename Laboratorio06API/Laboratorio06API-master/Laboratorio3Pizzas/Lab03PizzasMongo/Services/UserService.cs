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
        //Get a user with username and password
        public User GetUs(string password, string _user) => collection.Find<User>(x => x.Password == password && x.User_ == _user).FirstOrDefault();
        //Get a user with username
        public User GetUser(string _user) => collection.Find<User>(x => x.User_ == _user).FirstOrDefault();
        //Create a new user
        public User Post(User user)
        {
            collection.InsertOne(user);
            return user;
        }
        //Modificar una pizza deacuerdo al id
        public void Update(string id, User userMod) => collection.ReplaceOne(x => x.Id == id, userMod);
    }
}
