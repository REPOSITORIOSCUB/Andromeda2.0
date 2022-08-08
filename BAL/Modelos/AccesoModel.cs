using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos
{
    public class AccesoModel
    {
        public string usuario { get; set; }
        public string idMenu { get; set; }
        public string idPadre { get; set; }
        public string nomMenu { get; set; }
        public string ordenMenu { get; set; }
        public string controller { get; set; }
        public string imagen { get; set; }
        public string descripcion { get; set; }
        public string linkExterno { get; set; }
    }
}
