using BAL.Modelos;
using BAL.Modelos.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IRepositorioUsuario
    {
        bool Add(UsuairoModel obj);
        bool Edit(UsuairoModel obj);
        bool Delete(UsuairoModel obj);

        IEnumerable<InfoUsuariosModel> getUsuarios();
        IEnumerable<InfoUsuariosModel> getDatosPersona(string registro);

        UsuairoModel FindId(string uID);

        List<PerfilesModel> listaPerfilesActivos();
        bool ValidarCampos(UsuairoModel usuario, string v);

        string cifrarContrasena(string pwd);
    }
}
