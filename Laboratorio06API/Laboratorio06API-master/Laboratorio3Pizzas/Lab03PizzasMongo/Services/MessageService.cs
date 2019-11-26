using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersAPI.Models;
using MongoDB.Driver;
namespace UsersAPI.Services
{
    public class MessageService
    {
        private readonly IMongoCollection<Message> collection;

        public MessageService(IMessageDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            collection = database.GetCollection<Message>(settings.MessageCollectionName);
        }
       
        public Message GetMessage(string emisor, string receptor) => collection.Find<Message>(x => x.Emisor == emisor && x.Receptor == receptor).FirstOrDefault();
        //Create a new message
        public Message PostMessage(Message message)
        {
            collection.InsertOne(message);
            return message;
        }
    }
}
