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
    public class RepositorioBotonxPerXPag : IRepositorioBotonXPerXPag
    {
        #region Patro Singleton
        private static RepositorioBotonxPerXPag instance;

        public RepositorioBotonxPerXPag()
        {

        }
        public RepositorioBotonxPerXPag GetInstance()
        {
            if (instance == null)
            {
                instance = new RepositorioBotonxPerXPag();
            }
            return instance;
        }
        #endregion

        private OracleCommand _command;

        public bool Create(BotonXPerXPagModel obj)
        {
            _command = Metodos.CrearComandoProc("upb_pa2_coreapp.addperxboton");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vIdBoton", "INT").Value = obj.IdBoton;
            _command.Parameters["vIdBoton"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdPerfil", "INT").Value = obj.IdPerfil;
            _command.Parameters["vIdPerfil"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vBHabilitado", "INT").Value = 1;
            _command.Parameters["vBHabilitado"].Direction = ParameterDirection.Input;

            //_command.Parameters.Add("pFecregi", "NVARCHAR2").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// entidad.FechaRegistro;
            //_command.Parameters["pFecregi"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public bool Delete(BotonXPerXPagModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.deleteperxboton");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vId", "INT").Value = obj.Id;
            _command.Parameters["vId"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public bool Edit(BotonXPerXPagModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.editperxboton");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vId", "INT").Value = obj.Id;
            _command.Parameters["vId"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdPerfil", "INT").Value = obj.IdPerfil;
            _command.Parameters["vIdPerfil"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdBoton", "INT").Value = obj.IdBoton;
            _command.Parameters["vIdBoton"].Direction = ParameterDirection.Input;           

            _command.Parameters.Add("vBHabilitado", "INT").Value = obj.Estado;
            _command.Parameters["vBHabilitado"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public BotonXPerXPagModel FindId(string id)
        {
            BotonXPerXPagModel lista = new BotonXPerXPagModel();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.consultaridperxboton");
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
        

        public IEnumerable<BotonXPerXPagModel> getobj()
        {
            List<BotonXPerXPagModel> lista = new List<BotonXPerXPagModel>();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.listarperxboton");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vcursorgeneral", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);

            foreach (DataRow regi in dttLista.Rows)
            {
                lista.Add(LlenarEntidad(regi));
            }

            return lista;
        }

        public bool ValidarCampos(BotonXPerXPagModel BotonXPerXPag, string opcion)
        {
            bool returnValue = true;
            //string operacion =entidad.Operacion;
            switch (opcion)
            {
                case "save": { returnValue = Saveval(BotonXPerXPag); break; };
                case "edit": { returnValue = Editval(BotonXPerXPag); break; };
                default: { System.Console.WriteLine("Sin operacion Repositorio Modulo "); break; }
            }

            return returnValue;
        }

        private bool Editval(BotonXPerXPagModel botonXPerXPag)
        {
            bool returnValue = true;
            if (botonXPerXPag != null)
            {
                if ( /*string.IsNullOrEmpty(entidad.Cedula.ToString()) || */
                    string.IsNullOrEmpty(botonXPerXPag.IdBoton.ToString()) ||
                    string.IsNullOrEmpty(botonXPerXPag.IdPerfil.ToString()) ||
                    string.IsNullOrEmpty(botonXPerXPag.Estado.ToString())
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

        private bool Saveval(BotonXPerXPagModel botonXPerXPag)
        {
            bool returnValue = true;
            if (botonXPerXPag != null)
            {
                if ( /*string.IsNullOrEmpty(entidad.Cedula.ToString()) || */
                    string.IsNullOrEmpty(botonXPerXPag.IdBoton.ToString()) ||
                    string.IsNullOrEmpty(botonXPerXPag.IdPerfil.ToString()) ||
                    string.IsNullOrEmpty(botonXPerXPag.Estado.ToString())
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

        private BotonXPerXPagModel LlenarEntidad(DataRow Registro)
        {
            BotonXPerXPagModel obj = new BotonXPerXPagModel();
            obj.Id = Registro[0].ToString();
            obj.IdPerfil = Registro[1].ToString();
            obj.IdBoton = Registro[2].ToString();
            obj.Estado = Convert.ToInt32(Registro[3]);
            return obj;
        }
    }
}
