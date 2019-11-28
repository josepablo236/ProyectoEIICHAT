using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersAPI.Models
{
        public class MessageDatabaseSettings : IMessageDatabaseSettings
        {
            public string MessageCollectionName { get; set; }
            public string MConnectionString { get; set; }
            public string MDatabaseName { get; set; }
        }
        public interface IMessageDatabaseSettings
        {
            string MessageCollectionName { get; set; }
            string MConnectionString { get; set; }
            string MDatabaseName { get; set; }
        }
}
