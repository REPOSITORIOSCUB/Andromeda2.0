using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos.Configuracion
{
    public class MenuModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Id padre")]
        public string IdPadre { get; set; }
        [Display(Name = "Descripción")]
        public string Nombre { get; set; }
        [Display(Name = "Pápagina")]
        public string IdPagina { get; set; }
        [Display(Name = "Módulo")]
        public string IdModulo { get; set; }
        [Display(Name = "# Ordenamiento")]
        public int Ordenamiento { get; set; }

    }
}
