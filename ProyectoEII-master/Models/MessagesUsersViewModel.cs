using System;
using System.Collections.Generic;
using ProyectoEII.Models;

namespace ProyectoEII.Models
{
    public class MessagesUsersViewModel
    {
        public string ActualUser { get; set; }
        public List<UserViewModel> Users { get; set; }
        public List<MessagesViewModel> Messages { get; set; }

    }
}
