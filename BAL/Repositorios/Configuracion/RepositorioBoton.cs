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
    public class RepositorioBoton : IRepositorioBoton
    {
        #region Patron Singleton
       
        private static RepositorioBoton instance;
        public RepositorioBoton()
        {

        }

        public static RepositorioBoton getInstance()
        {
            if (instance == null)
            {
                instance = new RepositorioBoton();
            }
            return instance;
        }
        #endregion

        private OracleCommand _command;

        public bool Create(BotonesModel obj)
        {
            _command = Metodos.CrearComandoProc("upb_pa2_coreapp.AddBoton");
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

        public bool Delete(BotonesModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.DeleteBoton");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vId", "NVARCHAR2").Value = obj.Id;
            _command.Parameters["vId"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command); ;
        }

        public bool Edit(BotonesModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.EditBoton");
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

        public BotonesModel FindId(string id)
        {
            BotonesModel lista = new BotonesModel();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ConsultarIdBoton");
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

        private BotonesModel LlenarEntidad(DataRow registro)
        {
            BotonesModel obj = new BotonesModel();
            obj.Id = registro[0].ToString();
            obj.Nombre = registro[1].ToString();
            obj.Descripcion = registro[2].ToString();
            obj.Estado = Convert.ToInt32(registro[3]);
            return obj;
        }

        public IEnumerable<BotonesModel> getobj()
        {
            List<BotonesModel> lista = new List<BotonesModel>();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ListarBoton");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vcursorgeneral", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);

            foreach (DataRow regi in dttLista.Rows)
            {
                lista.Add(LlenarEntidad(regi));
            }


            return lista;
        }

        public bool ValidarCampos(BotonesModel boton, string evento)
        {
            bool returnValue = true;
            //string operacion =entidad.Operacion;
            switch (evento)
            {
                case "save": { returnValue = Saveval(boton); break; };
                case "edit": { returnValue = Editval(boton); break; };
                default: { System.Console.WriteLine("Sin operacion Repositorio Boton "); break; }
            }

            return returnValue;
        }

        private bool Editval(BotonesModel boton)
        {
            bool returnValue = true;
            if (boton != null)
            {
                if (
                    string.IsNullOrEmpty(boton.Nombre.ToString()) ||
                    string.IsNullOrEmpty(boton.Descripcion.ToString()) ||
                    string.IsNullOrEmpty(boton.Estado.ToString())
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

        private bool Saveval(BotonesModel boton)
        {
            bool returnValue = true;
            if (boton != null)
            {
                if (
                    string.IsNullOrEmpty(boton.Nombre.ToString()) ||
                    string.IsNullOrEmpty(boton.Descripcion.ToString()
                    )
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
