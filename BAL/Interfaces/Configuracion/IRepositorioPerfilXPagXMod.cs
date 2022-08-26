using BAL.Modelos.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces.Configuracion
{
    public interface IRepositorioPerfilXPagXMod
    {
        bool Create(PerfilXPagXModModel obj);
        bool Edit(PerfilXPagXModModel obj);
        bool Delete(PerfilXPagXModModel obj);
        IEnumerable<PerfilXPagXModModel> getobj();
        IEnumerable<PerfilXPagXModModel> getmodxper();
        PerfilXPagXModModel FindId(string id);
        bool ValidarCampos(PerfilXPagXModModel PerXPagXMod, string opcion);
    }
}
