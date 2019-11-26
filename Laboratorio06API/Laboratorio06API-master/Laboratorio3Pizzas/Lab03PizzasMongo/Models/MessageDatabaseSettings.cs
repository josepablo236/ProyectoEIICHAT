using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersAPI.Models
{
        public class MessageDatabaseSettings : IMessageDatabaseSettings
        {
            public string MessageCollectionName { get; set; }
            public string ConnectionString { get; set; }
            public string DatabaseName { get; set; }
        }
        public interface IMessageDatabaseSettings
        {
            string MessageCollectionName { get; set; }
            string ConnectionString { get; set; }
            string DatabaseName { get; set; }
        }
}
