using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Web;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos.General
{
    public class ArchivoModel
    {
        [Required]
        [Display(Name ="Archivo")]
        public HttpPostedFileBase ruta { get; set; }        

    }

}
