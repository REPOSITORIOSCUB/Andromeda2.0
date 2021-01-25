using BAL.Interfaces.Configuracion;
using BAL.Modelos.Configuracion;
using DAL;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Repositorios.Configuracion
{
    public class RepositorioPagina : IRepositorioPagina
    {
        #region Patron Singleton
        private static RepositorioPagina instance = null;

        public RepositorioPagina()
        {

        }

        public static  RepositorioPagina getInstance()
        {
            if (instance == null)
            {
                instance = new RepositorioPagina();
            }
            return instance;
        }
        #endregion

        private OracleCommand _command;

        public bool Create(PaginaModel obj)
        {
            _command = Metodos.CrearComandoProc("upb_pa2_coreapp.AddPagina");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vId", "INT").Value =Convert.ToInt32(obj.Id);
            _command.Parameters["vId"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vMensaje", "NVARCHAR2").Value = obj.Mensaje;
            _command.Parameters["vMensaje"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vAccion", "NVARCHAR2").Value = obj.Accion;
            _command.Parameters["vAccion"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vControlador", "NVARCHAR2").Value = obj.Controlador;
            _command.Parameters["vControlador"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vBHabilitado", "INT").Value = 1;
            _command.Parameters["vBHabilitado"].Direction = ParameterDirection.Input;

            //_command.Parameters.Add("pFecregi", "NVARCHAR2").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// entidad.FechaRegistro;
            //_command.Parameters["pFecregi"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public bool Delete(PaginaModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.DeletePagina");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vId", "INT").Value = obj.Id;
            _command.Parameters["vId"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command); ;
        }

        public bool Edit(PaginaModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.EditPagina");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vId", "INT").Value = obj.Id;
            _command.Parameters["vId"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vMensaje", "NVARCHAR2").Value = obj.Mensaje;
            _command.Parameters["vMensaje"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vAccion", "NVARCHAR2").Value = obj.Accion;
            _command.Parameters["vAccion"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vControlador", "NVARCHAR2").Value = obj.Controlador;
            _command.Parameters["vControlador"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vBHabilitado", "INT").Value = obj.Estado;
            _command.Parameters["vBHabilitado"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public PaginaModel FindId(string id)
        {
            PaginaModel lista = new PaginaModel();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ConsultarIdPagina");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vId", "NVARCHAR2").Value = id;
            _command.Parameters["vId"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vcursorgeneral", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);


            if (dttLista.Rows.Count > 0)
            {
                DataRow dr = dttLista.Rows[0];

                lista = LlenarEntidad(dr);

            }
            return lista;
        }

        private PaginaModel LlenarEntidad(DataRow registro)
        {
            PaginaModel obj =new  PaginaModel();
            obj.Id = Convert.ToInt32(registro[0]);
            obj.Mensaje = registro[1].ToString();
            obj.Accion = registro[2].ToString();
            obj.Controlador = registro[3].ToString();
            obj.Estado = Convert.ToInt32(registro[4]);
            return obj;

        }

        public IEnumerable<PaginaModel> getobj()
        {
            List<PaginaModel> lista = new List<PaginaModel>();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ListarPagina");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vcursorgeneral", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);

            foreach (DataRow regi in dttLista.Rows)
            {
                lista.Add(LlenarEntidad(regi));
            }


            return lista;
        }

        public bool ValidarCampos(PaginaModel pagina, string opcion)
        {
            bool returnValue = true;
            //string operacion =entidad.Operacion;
            switch (opcion)
            {
                case "save": { returnValue = Saveval(pagina); break; };
                case "edit": { returnValue = Editval(pagina); break; };
                default: { System.Console.WriteLine("Sin operacion Repositorio Pagina "); break; }
            }

            return returnValue;
        }

        private bool Editval(PaginaModel pagina)
        {
            bool returnValue = true;
            if (pagina != null)
            {
                if ( 
                    string.IsNullOrEmpty(pagina.Mensaje.ToString()) ||
                    string.IsNullOrEmpty(pagina.Accion.ToString()) ||
                    string.IsNullOrEmpty(pagina.Controlador.ToString())                     
                )
                {
                    returnValue = false;
                }
            }
            else
            {
                returnValue = false;
            }

            return returnValue;
        }

        private bool Saveval(PaginaModel pagina)
        {
            bool returnValue = true;
            if (pagina != null)
            {
                if (
                    
                    string.IsNullOrEmpty(pagina.Id.ToString()) ||
                    string.IsNullOrEmpty(pagina.Mensaje.ToString()) 
                    //string.IsNullOrEmpty(pagina.Accion.ToString()) ||
                    //string.IsNullOrEmpty(pagina.Controlador.ToString())                    
                )
                {
                    returnValue = false;
                }
            }
            else
            {
                returnValue = false;
            }

            return returnValue;
        }
    }
}
