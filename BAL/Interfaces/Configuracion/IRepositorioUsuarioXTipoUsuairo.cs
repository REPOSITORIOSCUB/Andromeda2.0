using BAL.Modelos.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces.Configuracion
{
    public interface IRepositorioUsuarioXTipoUsuairo
    {
        bool Create(UsuarioXTipoUsuarioModel obj);
        bool Edit(UsuarioXTipoUsuarioModel obj);
        bool Delete(UsuarioXTipoUsuarioModel obj);
        IEnumerable<UsuarioXTipoUsuarioModel> getobj();
        UsuarioXTipoUsuarioModel FindId(string id);
    }
}
