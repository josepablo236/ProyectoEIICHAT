using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab03PizzasMongo.Models
{
    public class PizzaDatabaseSettings : IPizzaDatabaseSettings
    {
        public string PizzaCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    public interface IPizzaDatabaseSettings
    {
        string PizzaCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
