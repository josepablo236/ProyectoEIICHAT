﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace UsersAPI.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Message")]
        public string Message_ { get; set; }
        public string Emisor { get; set; }
        public string Receptor { get; set; }
        public string File { get; set; }
        public DateTime Date { get; set; }
    }
}
