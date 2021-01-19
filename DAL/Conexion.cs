using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Conexion
    {
        static string cadenaconexion = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionOracleAndromeda"].ToString();

        public static string Cadenaconexion
        {
            get { return cadenaconexion; }
        }
    }
}
