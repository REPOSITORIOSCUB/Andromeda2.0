using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BAL.Modelos.Configuracion
{
    public class MenuModel
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Id padre")]
        public string IdPadre { get; set; }
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        [Display(Name = "Pápagina")]
        public string IdPagina { get; set; }
        [Display(Name = "Módulo")]
        public string IdModulo { get; set; }
        [Display(Name = "# Ordenamiento")]
        public int Ordenamiento { get; set; }
        [Display(Name = "Imagen")]
        public string Imagen { get; set; }
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }


        [Display(Name = "Archivo")]
        public HttpPostedFileBase ruta { get; set; }

        [Display(Name = "Pápagina padre")]
        public string IdPaginaPadre { get; set; }
    }
}
