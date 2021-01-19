using BAL.Modelos.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces.Configuracion
{
    public interface IRepositorioPerfiles
    {
        bool Add(PerfilesModel obj);
        bool Edit(PerfilesModel obj);
        bool Delete(PerfilesModel obj);

        IEnumerable<PerfilesModel> getPerfiles();


    }
}
