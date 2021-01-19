using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos.Configuracion
{
    public class InfoUsuariosModel
    {
        [Display(Name = "Cedula")]
        public string idetificadion { get; set; }

        [Display(Name = "Usuario")]
        public string usuario { get; set; }

        [Display(Name = "contraseña")]
        public string contrasena { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "Correo")]
        public string correo { get; set; }

        [Display(Name = "Estado")]
        public string estado { get; set; } 
        [Display(Name = "Estado Ges_Usu")]
        public string estadoGU { get; set; }



    }
}
