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
            var client = new MongoClient(settings.MConnectionString);
            var database = client.GetDatabase(settings.MDatabaseName);
            collection = database.GetCollection<Message>(settings.MessageCollectionName);
        }

        public List<Message> GetAll()
        {
            return collection.Find(x => true).ToList();
        }

        public List<Message> GetMessage(string emisor, string receptor)
        {
            var list1 = collection.Find<Message>(x => x.Emisor == emisor && x.Receptor == receptor).ToList();
            var list2 = collection.Find<Message>(x => x.Emisor == receptor && x.Receptor == emisor).ToList();
            var listMessages = new List<Message>();
            foreach (var item in list1)
            {
                listMessages.Add(item);
            }
            foreach (var item in list2)
            {
                listMessages.Add(item);
            }
            listMessages = listMessages.OrderBy(x => x.Date).ToList();
            return listMessages;
        }
        //Create a new message
        public Message PostMessage(Message message)
        {
            collection.InsertOne(message);
            return message;
        }
    }
}
