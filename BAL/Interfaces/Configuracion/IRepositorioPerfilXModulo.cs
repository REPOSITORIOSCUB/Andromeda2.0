using BAL.Modelos.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces.Configuracion
{
    public interface IRepositorioPerfilXModulo
    {
        bool Create(PerfilXModuloModel obj);
        bool Edit(PerfilXModuloModel obj);
        bool Delete(PerfilXModuloModel obj);
        IEnumerable<PerfilXModuloModel> getobj();
        PerfilXModuloModel FindId(string id);
        bool ValidarCampos(PerfilXModuloModel UsuXMod, string opcion);
    }
}
