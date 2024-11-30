using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen_Grupo2.Models
{
    public class Sitios
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public string firma { get; set; }
        public string audio { get; set; }
    }
}
