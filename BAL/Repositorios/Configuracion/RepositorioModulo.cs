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
    public class RepositorioModulo : IRepositorioModulo
    {
        #region "Patron Singleton"
        private static RepositorioModulo instance = null;

        public RepositorioModulo()
        {

        }

        public static RepositorioModulo getInstance()
        {
            if (instance == null)
            {
                instance = new RepositorioModulo();
            }

            return instance;
        }


        #endregion

        private OracleCommand _command;

        public bool Create(ModuloModel obj)
        {
            _command = Metodos.CrearComandoProc("upb_pa2_coreapp.AddModulo");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vNombre", "NVARCHAR2").Value = obj.Nombre;
            _command.Parameters["vNombre"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vBHabilitado", "INT").Value = 1;
            _command.Parameters["vBHabilitado"].Direction = ParameterDirection.Input;

            //_command.Parameters.Add("pFecregi", "NVARCHAR2").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// entidad.FechaRegistro;
            //_command.Parameters["pFecregi"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public bool Delete(ModuloModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.DeleteModulo");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vId", "INT").Value = obj.Id;
            _command.Parameters["vId"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public bool Edit(ModuloModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.EditModulo");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vId", "INT").Value = obj.Id;
            _command.Parameters["vId"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vNombre", "NVARCHAR2").Value = obj.Nombre;
            _command.Parameters["vNombre"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vBHabilitado", "INT").Value = obj.Estado;
            _command.Parameters["vBHabilitado"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public ModuloModel FindId(string id)
        {
            ModuloModel lista = new ModuloModel();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ConsultarIdModulo");
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

        public IEnumerable<ModuloModel> getobj()
        {
            List<ModuloModel> lista = new List<ModuloModel>();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ListarModulos");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vcursorgeneral", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);

            foreach (DataRow regi in dttLista.Rows)
            {
                lista.Add(LlenarEntidad(regi));
            }


            return lista;
        }

        private ModuloModel LlenarEntidad(DataRow registro)
        {
            ModuloModel obj = new  ModuloModel();
            obj.Id = registro[0].ToString();
            obj.Nombre = registro[1].ToString();
            obj.Estado = Convert.ToInt32(registro[2]);           
          
            return obj;
        }

        public bool ValidarCampos(ModuloModel modulo, string operacion)
        {
            bool returnValue = true;
            //string operacion =entidad.Operacion;
            switch (operacion)
            {
                case "save": { returnValue = Saveval(modulo); break; };
                case "edit": { returnValue = Editval(modulo); break; };
                default: { System.Console.WriteLine("Sin operacion Repositorio Modulo "); break; }
            }

            return returnValue;
        }

        private bool Editval(ModuloModel modulo)
        {
            bool returnValue = true;
            if (modulo != null)
            {
                if (
                    string.IsNullOrEmpty(modulo.Nombre.ToString()) 
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

        private bool Saveval(ModuloModel modulo)
        {
            bool returnValue = true;
            if (modulo != null)
            {
                if ( 
                    string.IsNullOrEmpty(modulo.Nombre.ToString()) 
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

        public IEnumerable<ModuloModel> getobjModuloxTiposuario(string vUsuario)
        {
            List<ModuloModel> lista = new List<ModuloModel>();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ListarModuloPerfil");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vUsuario", "NVARCHAR2").Value = vUsuario;
            _command.Parameters.Add("vCursorGeneral", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);

            foreach (DataRow regi in dttLista.Rows)
            {
                lista.Add(LlenarEntidadModulo(regi));
            }


            return lista;
        }

        private ModuloModel LlenarEntidadModulo(DataRow registro)
        {
            ModuloModel obj = new ModuloModel();
            obj.Id = registro[0].ToString();
            obj.Nombre = registro[1].ToString();
            string a  = registro[2].ToString();

            TipoUsuarioModel obj2 = new TipoUsuarioModel();
            obj2.Id = Convert.ToInt32(registro[2]);
            obj2.Nombre = registro[3].ToString();
            obj.tipousuario= obj2;
            obj2.Estado = Convert.ToInt32(registro[4]);

            return obj;
        }
    }
}
