using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos.Configuracion
{
    public class BotonXPerXPagModel
    {
        public string Id { get; set; }

        [Display(Name = "Perfil")]
        public string IdPerfil { get; set; }

        [Display(Name = "Botón")]
        public string IdBoton { get; set; }

        public int Estado { get; set; }
    }
}
