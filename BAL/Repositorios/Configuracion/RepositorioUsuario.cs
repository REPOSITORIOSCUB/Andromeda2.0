using BAL.Interfaces;
using BAL.Modelos;
using BAL.Modelos.Configuracion;
using DAL;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Repositorios.Configuracion
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        #region "Patron Singleton"
        private static RepositorioUsuario instance = null;

        public RepositorioUsuario()
        {

        }

        public static RepositorioUsuario getInstance()
        {
            if (instance == null)
            {
                instance = new RepositorioUsuario();
            }
            return instance;
        }
        #endregion

        private OracleCommand _command;

        public bool Add(UsuairoModel obj)
        {

            _command = Metodos.CrearComandoProc("upb_pa2_coreapp.AddUsuario");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vCedula", "NVARCHAR2").Value = obj.idpersona;
            _command.Parameters["vCedula"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vUsuario", "NVARCHAR2").Value = obj.Login;
            _command.Parameters["vUsuario"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vContrasena", "NVARCHAR2").Value = cifrarContrasena(obj.Password);
            _command.Parameters["vContrasena"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vPerfil", "INT").Value = Convert.ToInt32(obj.idtipousuario);
            _command.Parameters["vPerfil"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vBHabilitado", "INT").Value = 1;
            _command.Parameters["vBHabilitado"].Direction = ParameterDirection.Input;

            //_command.Parameters.Add("pFecregi", "NVARCHAR2").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// entidad.FechaRegistro;
            //_command.Parameters["pFecregi"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public string cifrarContrasena(string pwd)
        {
            string Clavecifrada = "";

            byte[] vBytes = Encoding.ASCII.GetBytes(pwd);
            SHA256 vSha = new SHA256Managed();
            byte[] vHash = vSha.ComputeHash(vBytes);
            foreach (byte b in vHash)
            {
                Clavecifrada += b.ToString("x2");
            }

            return Clavecifrada;
        }   

        public bool Delete(UsuairoModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.DeleteUsuario");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vidUsuario", "NVARCHAR2").Value = obj.idUsuairo;
            _command.Parameters["vidUsuario"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public bool Edit(UsuairoModel obj)
        {
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.EditUsuario");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vidUsuario", "NVARCHAR2").Value = obj.idUsuairo;
            _command.Parameters["vidUsuario"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vUsuario", "NVARCHAR2").Value = obj.Login;
            _command.Parameters["vUsuario"].Direction = ParameterDirection.Input;
           
            _command.Parameters.Add("vContrasena", "NVARCHAR2").Value = obj.Password;
            _command.Parameters["vContrasena"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vCedula", "NVARCHAR2").Value = obj.idpersona;
            _command.Parameters["vCedula"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vPerfil", "INT").Value = obj.idtipousuario;
            _command.Parameters["vPerfil"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vBHabilitado", "INT").Value = obj.bhabilitado;
            _command.Parameters["vBHabilitado"].Direction = ParameterDirection.Input;

            return Metodos.EjecutarComando(_command);
        }

        public IEnumerable<InfoUsuariosModel> getUsuarios()
        {
            List<InfoUsuariosModel> lista = new List<InfoUsuariosModel>();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ListarUsuarios");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vcursorusuario", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);

            foreach (DataRow regi in dttLista.Rows)
            {
                lista.Add(LlenarEntidad(regi));
            }

            //for (int i = 0; i < dttLista.Rows.Count; i++)
            //{
            //    DataRow dr = dttLista.Rows[i];

            //    Lista.Add(LlenarEntidad(dr));
            //}

            return lista;
        }

        private InfoUsuariosModel LlenarEntidad(DataRow registro)
        {
            InfoUsuariosModel obj = new InfoUsuariosModel();

            obj.idetificadion = registro[0].ToString();
            obj.usuario = registro[1].ToString();
            obj.nombre = registro[2].ToString();
            obj.correo = registro[3].ToString();
            obj.estado = registro[4].ToString();
            obj.estadoGU = registro[5].ToString();

            return obj;
        }

        public List<PerfilesModel> listaPerfilesActivos()
        {
            List<PerfilesModel> lista = new List<PerfilesModel>();
            DataTable dttLista = new DataTable();
            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ListaPerfilesActivos");
            _command.CommandType = CommandType.StoredProcedure;

            //_command.Parameters.Add("VESTADO", "NVARCHAR2").Value = estado;
            //_command.Parameters["codigo"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vCursorPerfiles", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);


            foreach (DataRow regi in dttLista.Rows)
            {
                lista.Add(LlenarPerfiles(regi));
            }



            return lista;
        }

        private PerfilesModel LlenarPerfiles(DataRow regi)
        {
            PerfilesModel obj = new PerfilesModel();
            obj.idtipouxm = Convert.ToInt32(regi[0]);
            obj.nombre = regi[1].ToString();

            return obj;
            ;
        }

        public UsuairoModel FindId(string uID)
        {

            UsuairoModel lista = new UsuairoModel();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ConsultarId");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vidUsuario", "NVARCHAR2").Value = uID;
            _command.Parameters["vidUsuario"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vcursorusuario", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);


            if (dttLista.Rows.Count > 0)
            {
                DataRow dr = dttLista.Rows[0];

                lista = LlenarEntidadU(dr);              
                
            }   
            return lista;
        }

        private UsuairoModel LlenarEntidadU(DataRow dr)
        {
            UsuairoModel obj = new UsuairoModel();

            obj.idUsuairo = Convert.ToInt32(dr[0].ToString());
            obj.idpersona = dr[3].ToString();
            obj.Login = dr[1].ToString();
            obj.Password = dr[2].ToString();
            obj.bhabilitado =Convert.ToInt32(dr[4].ToString());
            obj.idtipousuario = Convert.ToInt32(dr[5].ToString());
            

            return obj;
        }

        /// <summary>
        /// varificacion de los campos
        /// </summary>
        /// <param name="usuario">Entidad de Usuario</param>
        /// <param name="operacion">Valor de la operacion a realizar  Editar, agregar</param>
        /// <returns></returns>
        public bool ValidarCampos(UsuairoModel usuario, string operacion)
        {
            bool returnValue = true;
            //string operacion =entidad.Operacion;
            switch (operacion)
            {
                case "save": { returnValue = Saveval(usuario); break; };
                case "edit": { returnValue = Editval(usuario); break; };
                default: { System.Console.WriteLine("Sin operacion Repositorio Interventor "); break; }
            }

            return returnValue;
        }

        private bool Editval(UsuairoModel usuario)
        {
            bool returnValue = true;
            if (usuario != null)
            {
                if ( /*string.IsNullOrEmpty(entidad.Cedula.ToString()) || */
                    string.IsNullOrEmpty(usuario.idUsuairo.ToString()) || 
                    string.IsNullOrEmpty(usuario.Password.ToString()) || 
                    //string.IsNullOrEmpty(usuario.Perfil.ToString()) || 
                    string.IsNullOrEmpty(usuario.idpersona.ToString()) || 
                    string.IsNullOrEmpty(usuario.idtipousuario.ToString()) || 
                    string.IsNullOrEmpty(usuario.bhabilitado.ToString())
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

        private bool Saveval(UsuairoModel usuario)
        {
            bool returnValue = true;
            if (usuario != null)
            {
                if ( /*string.IsNullOrEmpty(entidad.Cedula.ToString()) ||*/
                    string.IsNullOrEmpty(usuario.idUsuairo.ToString()) ||
                    string.IsNullOrEmpty(usuario.Password.ToString()) ||
                    string.IsNullOrEmpty(usuario.Perfil.ToString()) ||
                    string.IsNullOrEmpty(usuario.idpersona.ToString()) ||
                    string.IsNullOrEmpty(usuario.bhabilitado.ToString())
                )
                {
                    returnValue = false;
                }
            }
            else
            {
                returnValue = false;
            }
            return returnValue; ;
        }

        public IEnumerable<InfoUsuariosModel> getDatosPersona(string dato)
        {
            List<InfoUsuariosModel> lista = new List<InfoUsuariosModel>();
            DataTable dttLista = new DataTable();

            _command = Metodos.CrearComandoProc("UPB_PA2_COREAPP.ListarDatosPersonas");
            _command.CommandType = CommandType.StoredProcedure;

            _command.Parameters.Add("vDato", "NVARCHAR2").Value = dato;
            _command.Parameters["vDato"].Direction = ParameterDirection.Input;

            _command.Parameters.Add("vcursorgeneral", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            dttLista = Metodos.EjecutarComandoSelect(_command);

            foreach (DataRow regi in dttLista.Rows)
            {
                lista.Add(LlenarInfoPersonas(regi));
            }            

            return lista;
        }

        //llena informacion consultada de personas
        private InfoUsuariosModel LlenarInfoPersonas(DataRow registro)
        {
            InfoUsuariosModel obj = new InfoUsuariosModel();

            obj.idetificadion = registro[0].ToString();           
            obj.nombre = registro[1].ToString();            

            return obj;
        }
    }
}
