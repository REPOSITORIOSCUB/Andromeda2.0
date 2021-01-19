using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class Metodos
    {
        public static OracleCommand CrearComando()
        {
            string _cadenaConexion = Conexion.Cadenaconexion;
            OracleConnection _conexion = new OracleConnection();
            _conexion.ConnectionString = _cadenaConexion;
            OracleCommand _comando = new OracleCommand();
            _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.Text;
            return _comando;
        }

        public static OracleCommand CrearComandoProc(string proc)
        {
            string _cadenaConexion = Conexion.Cadenaconexion;
            OracleConnection _conexion = new OracleConnection(_cadenaConexion);
            OracleCommand _comando = new OracleCommand(proc, _conexion);
            _comando.CommandType = CommandType.StoredProcedure;
            return _comando;
        }

        public static bool EjecutarComando(OracleCommand comando)
        {
            bool exito = true;
            int valor;
            try
            {
                comando.Connection.Open();
                valor = comando.ExecuteNonQuery();

                return exito;
            }
            catch (Exception smsError)
            {
                Ayudas objrnAyudas = new Ayudas();
                objrnAyudas.EscribirError(smsError.ToString());

                return false;
            }
            finally
            {
                comando.Connection.Dispose();
                comando.Connection.Close();
            }
        }

        public static DataTable EjecutarComandoSelect(OracleCommand comando)
        {
            DataTable _tabla = new DataTable();
            try
            {
                comando.Connection.Open();
                OracleDataAdapter adaptador = new OracleDataAdapter();
                adaptador.SelectCommand = comando;
                adaptador.Fill(_tabla);
            }
            catch (Exception smsError)
            {
                Ayudas objrnAyudas = new Ayudas();
                objrnAyudas.EscribirError(smsError.ToString());
            }
            finally
            { comando.Connection.Close(); }
            return _tabla;
        }
    }
}
