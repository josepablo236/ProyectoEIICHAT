using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Lab03PizzasMongo.Models
{
    public class Pizza
    {
        //private static readonly char delimiter = ',';
        //private string _tags;
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Nombre")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string TipoDeMasa { get; set; }
        public string Tamaño { get; set; }
        public int CantidadPorciones { get; set; }
        public string ExtraQueso { get; set; }
        //[NotMapped]
        public string[] Ingredientes { get; set; }

    }
}
