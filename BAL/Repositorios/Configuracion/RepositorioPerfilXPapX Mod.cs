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
    public class RepositorioPerfilXPapX_Mod : IRepositorioPerfilXPagXMod
    {
        #region Patron Singleton
        private static RepositorioPerfilXPapX_Mod instance;

        public RepositorioPerfilXPapX_Mod()
        {
                
        }

        public RepositorioPerfilXPapX_Mod GetIntance()
        {
            if (instance == null)
            {
                instance = new RepositorioPerfilXPapX_Mod();
            }
            return instance;
        }

        #endregion

        private OracleCommand _command;

        public bool Create(PerfilXPagXModModel obj)
        {
            _command = Metodos.CrearComandoProc("upb_pa2_coreapp.addperxpagxmod");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vIdPerfil", "INT").Value = obj.IdPerfil;
            _command.Parameters["vIdPerfil"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdPagina", "INT").Value = obj.IdPagina;
            _command.Parameters["vIdPerfil"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdModulo", "INT").Value = obj.IdModulo;
            _command.Parameters["vIdModulo"].Direction = ParameterDirection.Input;
           

            _command.Parameters.Add("vBHabilitado", "INT").Value = 1;
            _command.Parameters["vBHabilitado"].Direction = ParameterDirection.Input;

            //_command.Parameters.Add("pFecregi", "NVARCHAR2").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// entidad.FechaRegistro;
            //_command.Parameters["pFecregi"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public bool Delete(PerfilXPagXModModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.deleteperxpagxmod");
            _command.CommandType = CommandType.StoredProcedure;

            //_command.Parameters.Add("vId", "NVARCHAR2").Value = obj.Id;
            //_command.Parameters["vId"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdModulo", "INT").Value = obj.IdModulo;
            _command.Parameters["vIdModulo"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdPerfil", "INT").Value = obj.IdPerfil;
            _command.Parameters["vIdPerfil"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public bool Edit(PerfilXPagXModModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.editperxpagxmod");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vId", "INT").Value = obj.Id;
            _command.Parameters["vId"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdPerfil", "INT").Value = obj.IdPerfil;
            _command.Parameters["vIdPerfil"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdPagina", "INT").Value = obj.IdPagina;
            _command.Parameters["vIdPerfil"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vIdModulo", "INT").Value = obj.IdModulo;
            _command.Parameters["vIdModulo"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vBHabilitado", "INT").Value = obj.Estado;
            _command.Parameters["vBHabilitado"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public PerfilXPagXModModel FindId(string id)
        {
            PerfilXPagXModModel lista = new PerfilXPagXModModel();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.consultaridperxpagxmod");
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

       

        public IEnumerable<PerfilXPagXModModel> getobj()
        {
            List<PerfilXPagXModModel> lista = new List<PerfilXPagXModModel>();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.listarperxpagxmod");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vcursorgeneral", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);

            foreach (DataRow regi in dttLista.Rows)
            {
                lista.Add(LlenarEntidad(regi));
            }


            return lista;
        }

        public bool ValidarCampos(PerfilXPagXModModel PerXPagXMod, string opcion)
        {
            bool returnValue = true;
            //string operacion =entidad.Operacion;
            switch (opcion)
            {
                case "save": { returnValue = Saveval(PerXPagXMod); break; };
                case "edit": { returnValue = Editval(PerXPagXMod); break; };
                default: { System.Console.WriteLine("Sin operacion Repositorio PerfilXPagXMod "); break; }
            }

            return returnValue;
        }

        private bool Editval(PerfilXPagXModModel perXPagXMod)
        {
            bool returnValue = true;
            if (perXPagXMod != null)
            {
                if ( /*string.IsNullOrEmpty(entidad.Cedula.ToString()) || */
                   string.IsNullOrEmpty(perXPagXMod.IdModulo.ToString()) ||
                    string.IsNullOrEmpty(perXPagXMod.IdPagina.ToString()) ||
                    string.IsNullOrEmpty(perXPagXMod.IdPerfil.ToString()) ||
                    string.IsNullOrEmpty(perXPagXMod.Estado.ToString())
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

        private bool Saveval(PerfilXPagXModModel perXPagXMod)
        {
            bool returnValue = true;
            if (perXPagXMod != null)
            {
                if ( /*string.IsNullOrEmpty(entidad.Cedula.ToString()) || */
                    string.IsNullOrEmpty(perXPagXMod.IdModulo.ToString()) ||
                    string.IsNullOrEmpty(perXPagXMod.IdPagina.ToString()) ||
                    string.IsNullOrEmpty(perXPagXMod.IdPerfil.ToString()) ||
                    string.IsNullOrEmpty(perXPagXMod.Estado.ToString())
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

        private PerfilXPagXModModel LlenarEntidad(DataRow Registro)
        {
            PerfilXPagXModModel obj = new PerfilXPagXModModel();
            obj.Id = Registro[0].ToString();
            obj.IdPerfil = Registro[1].ToString();
            obj.IdPagina = Registro[2].ToString();
            obj.IdModulo = Registro[3].ToString();
            obj.Estado = Convert.ToInt32(Registro[4]);

            return obj; 
        }
    }
}
