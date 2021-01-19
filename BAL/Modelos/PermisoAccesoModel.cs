using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos
{
    public class PermisoAccesoModel
    {
        //public PermisoAccesoModel()
        //{
        //    this.MenuNavegacion = new List<AccesoModel>();
        //}
        public string usuario { get; set; }
        public string cedula { get; set; }
        public string tipousuario { get; set; }
        public string nommodulo { get; set; }

        //public List<AccesoModel> MenuNavegacion { get; set; }
    }
}
