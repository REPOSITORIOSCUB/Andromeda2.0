using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos.Configuracion
{
    public class BotonesModel
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public int Estado { get; set; }
    }
}
