using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos.General
{
    public class DatoAuditoria
    {
        [DisplayName("Usuario que Adiciona")]
        public string UsuarioAdiciona { get; set; }
        // SQ_DATOAUDITORIA_ID
        [DisplayName("Fecha Adición")]
        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public string FechaRegistro { get; set; }
        // public Nullable<System.DateTime> FechaRegistro { get; set; }

        [DisplayName("Usuario que Modifíca")]
        public string UsuarioModifica { get; set; }

        [DisplayName("Fecha Modifíca")]
        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public string FechaModificacion { get; set; }
    }
}
