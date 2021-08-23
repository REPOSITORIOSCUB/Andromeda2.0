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
    public class RepositorioUsuarioXTipoUsuairo : IRepositorioUsuarioXTipoUsuairo
    {
        #region Patron Singleton
        private static RepositorioUsuarioXTipoUsuairo instance = null;

        public RepositorioUsuarioXTipoUsuairo()
        {

        }

        public static RepositorioUsuarioXTipoUsuairo getInstance()
        {
            if (instance == null)
            {
                instance = new RepositorioUsuarioXTipoUsuairo();
            }
            return instance;
        }
        #endregion

        private OracleCommand _command;
        public bool Create(UsuarioXTipoUsuarioModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.addusuxtipo");
            _command.CommandType = CommandType.StoredProcedure;

            //_command.Parameters.Add("pId", "NUMBER").Value = obj.Id;
            //_command.Parameters["pId"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("pidentificacion", "NVARCHAR2").Value = obj.IdUsuario;
            _command.Parameters["pidentificacion"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("pidtipousuario", "NVARCHAR2").Value = obj.IdTipoUsuario;
            _command.Parameters["pidtipousuario"].Direction = ParameterDirection.Input;           

            _command.Parameters.Add("pusuadic", "NVARCHAR2").Value = obj.UsuarioAdiciona;
            _command.Parameters["pusuadic"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("pfecregi", "NVARCHAR2").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _command.Parameters["pfecregi"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public bool Delete(UsuarioXTipoUsuarioModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.deleteusuxtipo");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("pid", "NVARCHAR2").Value = obj.Id;
            _command.Parameters["pid"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public bool Edit(UsuarioXTipoUsuarioModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.editusuxtipo");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("pid", "INT").Value = obj.Id;
            _command.Parameters["pid"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("pnombre", "NVARCHAR2").Value = obj.IdUsuario;
            _command.Parameters["pnombre"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("pfechaini", "NVARCHAR2").Value = obj.IdTipoUsuario;
            _command.Parameters["pfechaini"].Direction = ParameterDirection.Input;           

            _command.Parameters.Add("pusuactu", "NVARCHAR2").Value = obj.UsuarioModifica;
            _command.Parameters["pusuactu"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("pfecactu", "NVARCHAR2").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _command.Parameters["pfecactu"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public UsuarioXTipoUsuarioModel FindId(string id)
        {
            UsuarioXTipoUsuarioModel lista = new UsuarioXTipoUsuarioModel();
            DataTable dttLista;

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.consultaridusuxtipo");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("pid", "NVARCHAR2").Value = id;
            _command.Parameters["pid"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("pcursorgeneral", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);


            if (dttLista.Rows.Count > 0)
            {
                DataRow registro = dttLista.Rows[0];

                lista = LlenarEntidad(registro);

            }
            return lista;
        }

        private UsuarioXTipoUsuarioModel LlenarEntidad(DataRow registro)
        {
            UsuarioXTipoUsuarioModel obj = new UsuarioXTipoUsuarioModel();
            obj.Id = registro[0].ToString();
            obj.IdUsuario = registro[1].ToString();
            obj.IdTipoUsuario = registro[2].ToString();

            obj.UsuarioAdiciona = registro[3].ToString();
            if (!string.IsNullOrEmpty(registro[4].ToString())) obj.FechaRegistro = registro[6].ToString();

            obj.UsuarioModifica = registro[5].ToString();
            if (!string.IsNullOrEmpty(registro[6].ToString())) obj.FechaRegistro = registro[9].ToString();

            return obj;
        }

        public IEnumerable<UsuarioXTipoUsuarioModel> getobj()
        {
            List<UsuarioXTipoUsuarioModel> lista = new List<UsuarioXTipoUsuarioModel>();
            DataTable dttLista;

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.listarusuxtipo");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vcursorgeneral", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);

            foreach (DataRow registro in dttLista.Rows)
            {
                lista.Add(LlenarEntidad(registro));
            }

            return lista;
        }
    }
}
