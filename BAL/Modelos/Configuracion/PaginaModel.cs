using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos.Configuracion
{
    public class PaginaModel
    {
        public int Id { get; set; }
        [Display(Name = "Nombre")]
        public string Mensaje { get; set; }
        [Display(Name = "Nombre página")]
        public string Accion { get; set; }
        [Display(Name = "Ruta")]
        public string Controlador { get; set; }
        public int Estado { get; set; }
        [Display(Name = "Usa Login")]
        public string linkExterno { get; set; }
    }
}
