using BAL.Modelos.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos.Configuracion
{
    public class ReportesModel : DatoAuditoria
    {
        public int Id { get; set; }
        [Display(Name = "Grupo")]
        public string Grupo { get; set; }
        public string Titulo { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        [Display(Name = "Ruta Enlace")]
        public string RutaEnlace { get; set; }
        [Display(Name = "Ruta Imagen")]
        public string RutaImagen { get; set; }
        public int Orden { get; set; }
        public int Activo { get; set; }
        
    }
}
