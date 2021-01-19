using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class Ayudas
    {
        string VarError;

        public string Error
        {
            get
            { return VarError; }
            set
            { VarError = value; }
        }

        public void EscribirError(string smsError)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\log\\log.txt";
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(path, true))
            {
                file.WriteLine(" ");
                file.WriteLine("Fecha: " + DateTime.Now);
                file.WriteLine("Descripción: " + smsError);
            }
        }

        internal static string ValidarObligatirio(string valorcampo, string nombrecampo)
        {
            if (valorcampo == string.Empty || valorcampo == " " || valorcampo == "")
            {
                return "* El campo " + nombrecampo + " es obligatorio";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
