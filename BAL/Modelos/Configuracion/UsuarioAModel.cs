using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos.Configuracion
{
    public class UsuarioAModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese su Usuario")]
        [Display(Name = "Usuario")]
        public string usuario { get; set; }

        [Required(ErrorMessage = "Ingrese su Contraseña")]
        [Display(Name = "Contraseña")]
        public string contraseña { get; set; }
       
        [Display(Name = "Cedula")]
        public string idPersona { get; set; }

        [Display(Name = "Estado")]
        public int estado { get; set; }



    }
}
