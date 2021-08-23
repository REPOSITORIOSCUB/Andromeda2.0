using BAL.Modelos.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos.Configuracion
{
    public class UsuarioXTipoUsuarioModel : DatoAuditoria    
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }
        [Display(Name = "Identificación")]
        public string IdUsuario { get; set; }
        [Display(Name = "Id Tipo Usuario")]
        public string IdTipoUsuario { get; set; }
        
    }
}
