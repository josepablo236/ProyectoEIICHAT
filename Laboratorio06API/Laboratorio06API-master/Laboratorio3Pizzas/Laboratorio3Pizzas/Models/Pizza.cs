using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Laboratorio3Pizzas.Models
{
    public class Pizza
    {
        private static readonly char delimiter = ',';
        private string _tags;

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string TipoDeMasa { get; set; }
        public string Tamaño { get; set; }
        public int CantidadPorciones { get; set; }
        public string ExtraQueso { get; set; }
        [NotMapped]
        public string[] Ingredientes { get; set; }

    }
}
