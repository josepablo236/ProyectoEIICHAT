using System.Collections.Generic;

namespace ProyectoEII.Models
{
    public class IndexMessagesViewModel
    {
        public string Usuario { get; set; }
        public string Busqueda { get; set; }
        public List<MessagesViewModel> listMessage { get; set; }
    }
}
