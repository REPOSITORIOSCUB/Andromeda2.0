using BAL.Modelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public List<AccesoModel> ConsultaUsuario(string usuario, string contrasena)
        {
            List<AccesoModel> Permisos = new List<AccesoModel>();
            string error = "";
            //string URL = "http://intra:8181/ApiAndromeda/"+usurio+"/"+contrasena;
            string URL = "http://localhost:2241/ApiAndromeda/" + usuario + "/"+contrasena;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    //Permisos.Add(JsonConvert.DeserializeObject<AccesoModel>(reader.ReadToEnd()));
                    string resp = reader.ReadToEnd();
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
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                   
                }
                return JsonConvert.DeserializeObject<List<AccesoModel>>(error);
            }

            //return Permisos;
        }

        public List<PermisoAccesoModel> ConsoltaPerModulo(string usuario)
        {
            List<PermisoAccesoModel> Permisos = new List<PermisoAccesoModel>();

            //string URL = "http://intra:8181/ApiAndromeda/"+usurio+"/"+contrasena;
            string URL = "http://localhost:2241/ApiAndromeda/GetAccesoModulos/" + usuario;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    //Permisos.Add(JsonConvert.DeserializeObject<AccesoModel>(reader.ReadToEnd()));
                    string resp = reader.ReadToEnd();

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
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();

                }
                return JsonConvert.DeserializeObject<List<PermisoAccesoModel>>("[{}]");
            }
        }
    }
}
