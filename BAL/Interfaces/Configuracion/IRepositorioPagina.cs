using BAL.Modelos.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces.Configuracion
{
    public interface IRepositorioPagina
    {
        bool Create(PaginaModel obj);
        bool Edit(PaginaModel obj);
        bool Delete(PaginaModel obj);
        IEnumerable<PaginaModel> getobj();
        PaginaModel FindId(string id);
        bool ValidarCampos(PaginaModel pagina, string opcion);
    }
}
