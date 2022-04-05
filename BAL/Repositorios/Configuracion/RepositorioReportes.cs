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
            throw new NotImplementedException();
        }

        public bool Delete(ReportesModel obj)
        {
            throw new NotImplementedException();
        }

        public bool Edit(ReportesModel obj)
        {
            throw new NotImplementedException();
        }

        public ReportesModel FindId(string id)
        {
            throw new NotImplementedException();
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
            obj.UsuarioAdiciona = registro["usuadic"].ToString();
            if (!string.IsNullOrEmpty(registro["fecregi"].ToString())) obj.FechaRegistro = registro["fecregi"].ToString();
            obj.UsuarioModifica = registro["usuactu"].ToString();
            if (!string.IsNullOrEmpty(registro["fecactu"].ToString())) obj.FechaModificacion = registro["fecactu"].ToString();

            return obj;
        }
    }
}
