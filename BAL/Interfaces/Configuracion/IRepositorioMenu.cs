using BAL.Modelos.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces.Configuracion
{
    public interface IRepositorioMenu
    {
        bool Create(MenuModel obj);
        bool Edit(MenuModel obj);
        bool Delete(MenuModel obj);
        IEnumerable<MenuModel> getobj();
        MenuModel FindId(string id);
        bool ValidarCampos(MenuModel menu, string opcion);
        
    }
}
