using BAL.Modelos.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces.Configuracion
{
    public interface IRepositorioReportes
    {
        bool Create(ReportesModel obj);
        bool Edit(ReportesModel obj);
        bool Delete(ReportesModel obj);
        IEnumerable<ReportesModel> getobj();
        ReportesModel FindId(string id);
    }
}
