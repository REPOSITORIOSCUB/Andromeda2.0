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
    public class RepositorioReportes : IRepositorioReportes
    {
        #region Patron Singleton

        private OracleCommand _command;

        private static RepositorioReportes instance;
        public RepositorioReportes()
        {

        }

        public static RepositorioReportes getInstance()
        {
            if (instance == null)
            {
                instance = new RepositorioReportes();
            }
            return instance;
        }
        #endregion



        public bool Create(ReportesModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.addreporte");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vGrupo", "NVARCHAR2").Value = obj.Grupo;
            _command.Parameters["vGrupo"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vTitulo", "NVARCHAR2").Value = obj.Titulo;
            _command.Parameters["vTitulo"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vdescripcion", "NVARCHAR2").Value = obj.Descripcion;
            _command.Parameters["vdescripcion"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vrutaenlace", "NVARCHAR2").Value = obj.RutaEnlace;
            _command.Parameters["vrutaenlace"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vimagen", "NVARCHAR2").Value = obj.RutaImagen;
            _command.Parameters["vimagen"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vOrden", "INT").Value = obj.Orden;
            _command.Parameters["vOrden"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vBHabilitado", "INT").Value = 1;
            _command.Parameters["vBHabilitado"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vUsuadic", "NVARCHAR2").Value = obj.UsuarioAdiciona.ToString();
            _command.Parameters["vUsuadic"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vFecregi", "NVARCHAR2").Value = obj.FechaRegistro.ToString();
            _command.Parameters["vFecregi"].Direction = ParameterDirection.Input;

            //_command.Parameters.Add("pFecregi", "NVARCHAR2").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// entidad.FechaRegistro;
            //_command.Parameters["pFecregi"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public bool Delete(ReportesModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.DeleteReporte");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vId", "INT").Value = obj.Id;
            _command.Parameters["vId"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public bool Edit(ReportesModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.EditReporte");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("pid", "INT").Value = obj.Id;
            _command.Parameters["pid"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vGrupo", "NVARCHAR2").Value = obj.Grupo;
            _command.Parameters["vGrupo"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vTitulo", "NVARCHAR2").Value = obj.Titulo;
            _command.Parameters["vTitulo"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vdescripcion", "NVARCHAR2").Value = obj.Descripcion;
            _command.Parameters["vdescripcion"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vrutaenlace", "NVARCHAR2").Value = obj.RutaEnlace;
            _command.Parameters["vrutaenlace"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vimagen", "NVARCHAR2").Value = obj.RutaImagen;
            _command.Parameters["vimagen"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vOrden", "NUMBER").Value =  obj.Orden;
            _command.Parameters["vOrden"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vBHabilitado", "INT").Value = obj.Activo;
            _command.Parameters["vBHabilitado"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vUsuactuc", "NVARCHAR2").Value = obj.UsuarioModifica.ToString();
            _command.Parameters["vUsuactuc"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vFecactu", "NVARCHAR2").Value = obj.FechaModificacion.ToString();
            _command.Parameters["vFecactu"].Direction = ParameterDirection.Input;


            return Metodos.EjecutarComando(_command);
        }

        public ReportesModel FindId(string id)
        {
            ReportesModel lista = new ReportesModel();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ConsultarIdReporte");
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

        //optiene todos los reportes
        public IEnumerable<ReportesModel> getobj()
        {
            List<ReportesModel> lista = new List<ReportesModel>();
            DataTable dttLista;

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.listarreportes");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("pcursorgeneral", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);

            foreach (DataRow registro in dttLista.Rows)
            {
                lista.Add(LlenarEntidad(registro));
            }


            return lista;
        }

        /// <summary>
        /// llena el objeto  reporte
        /// </summary>
        /// <param name="registro">objeto de datos  reporte</param>
        /// <returns>objeto lleno con la informacion del reporte</returns>
        private ReportesModel LlenarEntidad(DataRow registro)
        {
            ReportesModel obj = new ReportesModel();
            obj.Id = Convert.ToInt32(registro["id"]);
            obj.Titulo = registro["titulo"].ToString();
            obj.Descripcion = registro["descripcion"].ToString();
            obj.RutaEnlace = registro["rutaenlace"].ToString();
            obj.RutaImagen = registro["imagen"].ToString();
            obj.Orden = Convert.ToInt32(registro["orden"]);
            obj.Activo = Convert.ToInt32(registro["bhabilitado"]);
            obj.UsuarioAdiciona = registro["usuadic"].ToString();
            if (!string.IsNullOrEmpty(registro["fecregi"].ToString())) obj.FechaRegistro = registro["fecregi"].ToString();
            obj.UsuarioModifica = registro["usuactu"].ToString();
            if (!string.IsNullOrEmpty(registro["fecactu"].ToString())) obj.FechaModificacion = registro["fecactu"].ToString();
            obj.Grupo = registro["grupo"].ToString();
            return obj;
        }
    }
}
