using BAL.Modelos.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces.Configuracion
{
    public interface IRepositorioBoton
    {
        bool Create(BotonesModel obj);
        bool Edit(BotonesModel obj);
        bool Delete(BotonesModel obj);
        IEnumerable<BotonesModel> getobj();
        BotonesModel FindId(string id);
        bool ValidarCampos(BotonesModel boton, string evento);
    }
}
