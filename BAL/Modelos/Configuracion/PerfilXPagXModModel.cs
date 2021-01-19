using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos.Configuracion
{
    public class PerfilXPagXModModel
    {
        public string Id { get; set; }

        [Display(Name = "Perfil")]
        public string IdPerfil { get; set; }

        [Display(Name = "Página")]
        public string IdPagina { get; set; }

        [Display(Name = "Módulo")]
        public string IdModulo { get; set; }
        public int Estado { get; set; }
    }
}
