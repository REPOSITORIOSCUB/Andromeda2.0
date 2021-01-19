using BAL.Modelos.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces.Configuracion
{
    public interface IRepositorioTipoUsuario
    {
        bool Create(TipoUsuarioModel obj);
        bool Edit(TipoUsuarioModel obj);
        bool Delete(TipoUsuarioModel obj);
        IEnumerable<TipoUsuarioModel> getobj();
        TipoUsuarioModel FindId(string uID);
        bool ValidarCampos(TipoUsuarioModel tipousuario, string v);
    }
}
