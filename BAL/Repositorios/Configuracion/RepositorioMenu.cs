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
    public class RepositorioMenu : IRepositorioMenu
    {
        #region Patron Singleton

        private static RepositorioMenu instance;
        public RepositorioMenu()
        {

        }

        public static RepositorioMenu getInstance()
        {
            if (instance == null)
            {
                instance = new RepositorioMenu();
            }
            return instance;
        }
        #endregion

        private OracleCommand _command;

        public bool Create(MenuModel obj)
        {
            _command = Metodos.CrearComandoProc("upb_pa2_coreapp.AddMenu");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vNombre", "NVARCHAR2").Value = obj.Nombre;
            _command.Parameters["vNombre"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vOrdenamiento", "INT").Value = obj.Ordenamiento;
            _command.Parameters["vOrdenamiento"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdModulo", "INT").Value = obj.IdModulo;
            _command.Parameters["vIdModulo"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdPadre", "INT").Value = obj.IdPadre;
            _command.Parameters["vIdPadre"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdPagina", "INT").Value = obj.IdPagina;
            _command.Parameters["vIdPagina"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vImg", "NVARCHAR2").Value = obj.Imagen;
            _command.Parameters["vImg"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vDescripcion", "NVARCHAR2").Value = obj.Descripcion;
            _command.Parameters["vDescripcion"].Direction = ParameterDirection.Input;

            //_command.Parameters.Add("pFecregi", "NVARCHAR2").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// entidad.FechaRegistro;
            //_command.Parameters["pFecregi"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public bool Delete(MenuModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.DeleteMenu");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vId", "INT").Value = obj.Id;
            _command.Parameters["vId"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public bool Edit(MenuModel obj)
        {
            _command = Metodos.CrearComandoProc("upb_pa2_coreapp.EditMenu");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vId", "INT").Value = obj.Id;
            _command.Parameters["vId"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vNombre", "NVARCHAR2").Value = obj.Nombre;
            _command.Parameters["vNombre"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vOrdenamiento", "INT").Value = obj.Ordenamiento;
            _command.Parameters["vOrdenamiento"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdModulo", "INT").Value = obj.IdModulo;
            _command.Parameters["vIdModulo"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdPadre", "INT").Value = obj.IdPadre;
            _command.Parameters["vIdPadre"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdPagina", "INT").Value = obj.IdPagina;
            _command.Parameters["vIdPagina"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vImg", "NVARCHAR2").Value = obj.Imagen;
            _command.Parameters["vImg"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vDescripcion", "NVARCHAR2").Value = obj.Descripcion;
            _command.Parameters["vDescripcion"].Direction = ParameterDirection.Input;



            //_command.Parameters.Add("pFecregi", "NVARCHAR2").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// entidad.FechaRegistro;
            //_command.Parameters["pFecregi"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public MenuModel FindId(string Id)
        {
            MenuModel lista = new MenuModel();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ConsultarIdMenu");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vId", "NVARCHAR2").Value = Id;
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

        public IEnumerable<MenuModel> getobj()
        {
            List<MenuModel> lista = new List<MenuModel>();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ListarMenu");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vcursorgeneral", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);

            foreach (DataRow regi in dttLista.Rows)
            {
                lista.Add(LlenarEntidad(regi));
            }


            return lista;
        }

        private MenuModel LlenarEntidad(DataRow regi)
        {
            MenuModel obj = new MenuModel();
            obj.Id = regi["idmenu"].ToString();
            obj.IdPadre = regi["idpadre"].ToString();
            obj.Nombre = regi["nombre"].ToString();
            obj.IdPagina = regi["idpagina"].ToString();
            obj.IdModulo = regi["idmodulo"].ToString();
            obj.Ordenamiento = Convert.ToInt32(regi["orden"]);
            obj.Imagen = regi["imagen"].ToString();
            obj.Descripcion = regi["descripcion"].ToString();
            obj.IdPaginaPadre = regi["idpaginaPadre"].ToString();
            return obj;
        }

        public bool ValidarCampos(MenuModel menu, string opcion)
        {
            bool returnValue = true;
            //string operacion =entidad.Operacion;
            switch (opcion)
            {
                case "save": { returnValue = Saveval(menu); break; };
                case "edit": { returnValue = Editval(menu); break; };
                default: { System.Console.WriteLine("Sin operacion Repositorio Interventor "); break; }
            }

            return returnValue;
        }

        private bool Editval(MenuModel menu)
        {
            bool returnValue = true;
            if (menu != null)
            {
                if ( /*string.IsNullOrEmpty(entidad.Cedula.ToString()) || */
                    string.IsNullOrEmpty(menu.Nombre.ToString()) ||                    
                    string.IsNullOrEmpty(menu.Ordenamiento.ToString())
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

        private bool Saveval(MenuModel menu)
        {
            bool returnValue = true;
            if (menu != null)
            {
                if ( /*string.IsNullOrEmpty(entidad.Cedula.ToString()) || */
                    string.IsNullOrEmpty(menu.Nombre.ToString()) ||
                    string.IsNullOrEmpty(menu.Ordenamiento.ToString())
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
