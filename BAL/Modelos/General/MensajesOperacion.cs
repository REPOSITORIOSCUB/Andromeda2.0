using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Modelos.General
{
    public class MensajesOperacion
    {
        public string Mensaje { get; set; }
        public string TipoMsg { get; set; }
        public bool Muestra { get; set; }
        public bool Mostro { get; set; }

        //public void MensajesOperacion();

        public MensajesOperacion MensajeVista(int mensajesVista, string nameController)
        {
            MensajesOperacion mensajes = new MensajesOperacion();

            switch (mensajesVista)
            {
                case 1:
                    mensajes.Mensaje = "Se ingreso un nuevo " + nameController;
                    mensajes.TipoMsg = "success";
                    break;
                case 2:
                    mensajes.Mensaje = "Se presento un error al momento de Guardar";
                    mensajes.TipoMsg = "warning";
                    break;
                case 3:
                    mensajes.Mensaje = "Hay campos sin diligenciar para el ingreso de la información ";
                    mensajes.TipoMsg = "warning";
                    break;
                case 4:
                    mensajes.Mensaje = "Se producjo un error al ingresar un nuevo " + nameController;
                    mensajes.TipoMsg = "error";
                    break;
                case 5:
                    mensajes.Mensaje = "Error al Cargar la informacion Solicitada";
                    mensajes.TipoMsg = "warning";
                    break;
                case 6:
                    mensajes.Mensaje = "Se Actualizo! la informacion con Exito!";
                    mensajes.TipoMsg = "success";
                    break;

                case 7:
                    mensajes.Mensaje = "Se presento un error! al Actualizar la informacion Solicitada";
                    mensajes.TipoMsg = "error";
                    break;

                case 8:
                    mensajes.Mensaje = "Se presento un error! al elimianar la informacion Solicitada";
                    mensajes.TipoMsg = "error";
                    break;

                case 9:
                    mensajes.Mensaje = "Se  elimianó la informacion Solicitada";
                    mensajes.TipoMsg = "success";
                    break;
             

                default:
                    mensajes.Mensaje = "";
                    mensajes.TipoMsg = "";
                    break;
            }

            if (mensajesVista >= 1 && mensajesVista <= 9) mensajes.Muestra = true;
            else mensajes.Muestra = false;

            return mensajes;
        }
    }
}
