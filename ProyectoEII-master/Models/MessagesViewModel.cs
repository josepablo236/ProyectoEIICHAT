using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoEII.Models
{
    public class MessagesViewModel
    {
        public string Emisor { get; set; }
        public  string Receptor { get; set; }
        public string Message_ { get; set; }
        public string File { get; set; }
        public DateTime Date { get; set; }

    }
}
