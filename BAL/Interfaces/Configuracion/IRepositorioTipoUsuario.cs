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
        bool Eliminar(string idpersona);
        bool Grabar(string idpersona, string idmodulo, string idperfil);
        IEnumerable<TipoUsuarioModel> getobj();

        IEnumerable<TipoUsuarioModel> getTipoUsuairoSinAsignar();

        TipoUsuarioModel FindId(string uID);
        bool ValidarCampos(TipoUsuarioModel tipousuario, string v);
    }
}
