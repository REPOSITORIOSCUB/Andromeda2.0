using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos
{
   [Table("PA_CORE_USUARIOS")]
    public class UsuairoModel
    {

        public UsuairoModel()
        {
           
        }

        [Display(Name = "id")]
        public int idUsuairo { get; set; }

        [Required(ErrorMessage = "Ingrese su Usuario")]
        [Display(Name = "Usuario")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Ingrese la contraseña")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Identificación")]
        public string idpersona { get; set; }

        [Display(Name = "Estado")]
        public int bhabilitado { get; set; }
        [Display(Name = "Perfil")]
        public int idtipousuario { get; set; }

        public List<AccesoModel> Perfil { get; set; }
    }
}
