using BAL.Modelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosApp
{
    public class ConsultaPerfilUsuario
    {
        #region "Patron Singleton"
        public static ConsultaPerfilUsuario instancia = null;
        public ConsultaPerfilUsuario() { }

        public static ConsultaPerfilUsuario getIntancia() {
            if (instancia == null)
            {
                instancia = new ConsultaPerfilUsuario();
            }
            return instancia;
        
        }
        #endregion

        public List<AccesoModel> ConsultaUsuario(string usuario, string contrasena, string nommodulo)
        {
            List<AccesoModel> Permisos = new List<AccesoModel>();
            string error = "";  
            //Direccion api
            string URL = ConfigurationManager.AppSettings["ApiAndromeda"].ToString() + usuario + "/" + contrasena + "/" + nommodulo;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    //Permisos.Add(JsonConvert.DeserializeObject<AccesoModel>(reader.ReadToEnd()));
                    string resp = reader.ReadToEnd();
                    //dynamic parsedJson = JsonConvert.DeserializeObject(resp);
                    error = resp;
                    if (resp == "Clave Erronea" || resp == "[{}]" || String.IsNullOrEmpty(resp))
                    {
                        List<AccesoModel> lista = new List<AccesoModel>();
                        return lista ;
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<List<AccesoModel>>(resp);
                    }                    
                    
                }
            }
            catch (WebException )
            {               
                return JsonConvert.DeserializeObject<List<AccesoModel>>(error);
            }

            //return Permisos;
        }

        public List<PermisoAccesoModel> ConsoltaPerModulo(string usuario)
        {
            List<PermisoAccesoModel> Permisos = new List<PermisoAccesoModel>();
            string URL = ConfigurationManager.AppSettings["ApiAndromeda"].ToString() + "GetAccesoModulos/" + usuario;
            string error = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    //Permisos.Add(JsonConvert.DeserializeObject<AccesoModel>(reader.ReadToEnd()));
                    string resp = reader.ReadToEnd();                    
                    //  dynamic parsedJson = JsonConvert.DeserializeObject(resp);
                    error = resp;
                    if (resp == "[{}]" || String.IsNullOrEmpty(resp))
                    {
                        List<PermisoAccesoModel> lista = new List<PermisoAccesoModel>();
                        return lista;
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<List<PermisoAccesoModel>>(resp);
                    }

                }
            }
            catch (WebException )
            {              
               
                return JsonConvert.DeserializeObject<List<PermisoAccesoModel>>(error);
            }
        }
    }
}
