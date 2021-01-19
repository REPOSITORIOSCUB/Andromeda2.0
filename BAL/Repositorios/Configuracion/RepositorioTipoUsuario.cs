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
   public class RepositorioTipoUsuario : IRepositorioTipoUsuario
    {
        #region Patron Singleton
        private static RepositorioTipoUsuario instance = null;

        public RepositorioTipoUsuario()
        {

        }

        public static RepositorioTipoUsuario getInstance()
        {
            if (instance == null)
            {
                instance = new RepositorioTipoUsuario();
            }
            return instance;
        }
        #endregion

        private OracleCommand _command;

        public bool Create(TipoUsuarioModel obj)
        {
            _command = Metodos.CrearComandoProc("upb_pa2_coreapp.AddTipoUsuario");
            _command.CommandType = CommandType.StoredProcedure;           

            _command.Parameters.Add("vNombre", "NVARCHAR2").Value = obj.Nombre;
            _command.Parameters["vNombre"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vDescripcion", "NVARCHAR2").Value = obj.Descripcion;
            _command.Parameters["vDescripcion"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vBHabilitado", "INT").Value = 1;
            _command.Parameters["vBHabilitado"].Direction = ParameterDirection.Input;

            //_command.Parameters.Add("pFecregi", "NVARCHAR2").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// entidad.FechaRegistro;
            //_command.Parameters["pFecregi"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public bool Delete(TipoUsuarioModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.DeleteTipoUsuario");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vidUsuario", "INT").Value = obj.Id;
            _command.Parameters["vidUsuario"].Direction = ParameterDirection.Input;         

            return Metodos.EjecutarComando(_command);
        }

        public bool Edit(TipoUsuarioModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.EditTipoUsuario");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vId", "INT").Value = obj.Id;
            _command.Parameters["vId"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vNombre", "NVARCHAR2").Value = obj.Nombre;
            _command.Parameters["vNombre"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vDescripcion", "NVARCHAR2").Value = obj.Descripcion;
            _command.Parameters["vDescripcion"].Direction = ParameterDirection.Input;            

            _command.Parameters.Add("vBHabilitado", "INT").Value = obj.Estado;
            _command.Parameters["vBHabilitado"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public TipoUsuarioModel FindId(string Id)
        {
            TipoUsuarioModel lista = new TipoUsuarioModel();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ConsultarIdTipUsu");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vidUsuario", "NVARCHAR2").Value = Id;
            _command.Parameters["vidUsuario"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vcursorusuario", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);


            if (dttLista.Rows.Count > 0)
            {
                DataRow dr = dttLista.Rows[0];

                lista = LlenarEntidad(dr);

            }
            return lista;
        }

        public IEnumerable<TipoUsuarioModel> getobj()
        {
            List<TipoUsuarioModel> lista= new List<TipoUsuarioModel>();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ListarTiposUsuarios");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vcursorgeneral", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);

            foreach (DataRow regi in dttLista.Rows)
            {
                lista.Add(LlenarEntidad(regi));
            }


            return lista;
        }
        private TipoUsuarioModel LlenarEntidad(DataRow registro)
        {
            TipoUsuarioModel obj = new TipoUsuarioModel();

            obj.Id = Convert.ToInt32(registro[0]);
            obj.Nombre = registro[1].ToString();
            obj.Descripcion = registro[2].ToString();
            obj.Estado = Convert.ToInt32(registro[3].ToString());           

            return obj;
        }

        public bool ValidarCampos(TipoUsuarioModel tipousuario, string operacion)
        {
            bool returnValue = true;
            //string operacion =entidad.Operacion;
            switch (operacion)
            {
                case "save": { returnValue = Saveval(tipousuario); break; };
                case "edit": { returnValue = Editval(tipousuario); break; };
                default: { System.Console.WriteLine("Sin operacion Repositorio Interventor "); break; }
            }

            return returnValue;
        }

        private bool Saveval(TipoUsuarioModel tipousuario)
        {
            bool returnValue = true;
            if (tipousuario != null)
            {
                if ( 
                    string.IsNullOrEmpty(tipousuario.Nombre.ToString()) ||
                    string.IsNullOrEmpty(tipousuario.Descripcion.ToString())                     
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

        private bool Editval(TipoUsuarioModel tipousuario)
        {
            bool returnValue = true;
            if (tipousuario != null)
            {
                if ( /*string.IsNullOrEmpty(entidad.Cedula.ToString()) || */
                    string.IsNullOrEmpty(tipousuario.Nombre.ToString()) ||
                    string.IsNullOrEmpty(tipousuario.Descripcion.ToString()) ||
                    //string.IsNullOrEmpty(usuario.Perfil.ToString()) ||                     
                    string.IsNullOrEmpty(tipousuario.Estado.ToString())
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
