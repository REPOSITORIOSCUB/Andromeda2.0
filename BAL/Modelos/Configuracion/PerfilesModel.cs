using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos.Configuracion
{
    [Table("PA_CORE_USUARIOS")]
    public class PerfilesModel
    {
        [Display(Name = "Id")]
        public int idtipouxm { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

        [Display(Name = "Activo")]
        public int bhabilitado { get; set; }


    }
}
