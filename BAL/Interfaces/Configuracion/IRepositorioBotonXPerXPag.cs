using BAL.Modelos.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces.Configuracion
{
    public interface IRepositorioBotonXPerXPag
    {
        bool Create(BotonXPerXPagModel obj);
        bool Edit(BotonXPerXPagModel obj);
        bool Delete(BotonXPerXPagModel obj);
        IEnumerable<BotonXPerXPagModel> getobj();
        BotonXPerXPagModel FindId(string id);
        bool ValidarCampos(BotonXPerXPagModel BotonXPerXPag, string opcion);
    }
}
