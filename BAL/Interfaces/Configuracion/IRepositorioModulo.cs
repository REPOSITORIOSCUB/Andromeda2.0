using BAL.Modelos.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces.Configuracion
{
    public interface IRepositorioModulo
    {
        bool Create(ModuloModel obj);
        bool Edit(ModuloModel obj);
        bool Delete(ModuloModel obj);
        IEnumerable<ModuloModel> getobj();
        ModuloModel FindId(string uID);
        bool ValidarCampos(ModuloModel modulo, string v);
    }
}
