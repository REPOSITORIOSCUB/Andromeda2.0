using BAL.Interfaces;
using BAL.Modelos;
using BAL.Modelos.Configuracion;
using BAL.Modelos.General;
using BAL.Repositorios.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppAndromedaCore.Controllers
{
    public class UsuariosController : Controller
    {
        IRepositorioUsuario _repositorioUsuario;

        public UsuariosController()
        {
            if (_repositorioUsuario == null)
            {
                _repositorioUsuario = new RepositorioUsuario();
            }
        }

        // GET: Usurios
        public ActionResult Index()
        {
            if (verificarSession())
            {
                MensajesOperacion mensajes = new MensajesOperacion();
                string showMsg = "";

                try
                {
                    if (TempData["Message"] != null) mensajes.Mensaje = TempData["Message"].ToString();
                    if (TempData["AlertType"] != null) mensajes.TipoMsg = TempData["AlertType"].ToString();
                    if (TempData["ShowAlert"] != null) mensajes.Muestra = (TempData["ShowAlert"].ToString().ToLower().Equals("true")) ? true : false;
                    if (TempData["ShowMsg"] != null)
                    {
                        mensajes.Mostro = (TempData["ShowMsg"].ToString().ToLower().Equals("s")) ? true : false;
                        showMsg = TempData["ShowMsg"].ToString();
                    }

                }
                catch { }

                ViewBag.Message = mensajes.Mensaje;
                ViewBag.AlertType = mensajes.TipoMsg;
                ViewBag.ShowAlert = mensajes.Muestra.ToString();
                //if (muestraMsg.Equals("N")) 
                ViewBag.ShowMsg = (mensajes.Mostro.ToString().ToLower().Equals("true")) ? "S" : "N";

                return View(_repositorioUsuario.getUsuarios());
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }      
           
            
        }

        public ActionResult Create()
        {
            if (verificarSession())
            {
                List<SelectListItem> itemsPerfil = new SelectList(_repositorioUsuario.listaPerfilesActivos(), "idtipouxm", "nombre").ToList();
                itemsPerfil.Insert(0, (new SelectListItem { Text = " [Seleccione Perfíl] ", Value = null }));

                ViewBag.ListOfPerfiles = new SelectList(itemsPerfil.ToList(), "value", "Text", "");

                UsuairoModel data = new UsuairoModel();

                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }           
        }       

        [HttpPost]
        public ActionResult Create(UsuairoModel usuario)
        {
            if (verificarSession())
            {
                int mensajesVista = 0;
                string showMsg = "";
                MensajesOperacion mensajes = new MensajesOperacion();
                MensajesOperacion msgAnter = new MensajesOperacion();


                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                try
                {
                    if (TempData["Message"] != null) msgAnter.Mensaje = TempData["Message"].ToString();
                    if (TempData["AlertType"] != null) msgAnter.TipoMsg = TempData["AlertType"].ToString();
                    if (TempData["ShowAlert"] != null) msgAnter.Muestra = (TempData["ShowAlert"].ToString().ToLower().Equals("true")) ? true : false;
                    if (TempData["ShowMsg"] != null)
                    {
                        msgAnter.Mostro = (TempData["ShowMsg"].ToString().ToLower().Equals("s")) ? true : false;
                        showMsg = TempData["ShowMsg"].ToString().ToLower();
                    }
                }
                catch (Exception) { throw; }
                try
                {
                    if (_repositorioUsuario.Add(usuario))
                    {
                        mensajesVista = 1;
                    }
                    else
                    {
                        mensajesVista = 2;
                    }


                }
                catch (Exception)
                {

                    mensajesVista = 2;
                }

                mensajes = mensajes.MensajeVista(mensajesVista, controllerName);
                mensajes.Mensaje = mensajes.Mensaje + " [ " + actionName + " / " + controllerName + " ] ";

                TempData["Message"] = mensajes.Mensaje;
                TempData["AlertType"] = mensajes.TipoMsg;
                TempData["ShowAlert"] = mensajes.Muestra.ToString();
                TempData["ShowMsg"] = (mensajes.Mostro.ToString().ToLower().Equals("true")) ? "S" : "N";

                ViewBag.Message = mensajes.Mensaje;
                ViewBag.AlertType = mensajes.TipoMsg;
                ViewBag.ShowAlert = mensajes.Muestra.ToString();
                ViewBag.ShowMsg = (mensajes.Mostro.ToString().ToLower().Equals("true")) ? "S" : "N";

                if (mensajes.TipoMsg.Equals("success"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(usuario);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
            
        }


        public ActionResult Edit(string  id)
        {
            if (verificarSession())
            {
                //variables           
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                MensajesOperacion mensajes = new MensajesOperacion();
                MensajesOperacion msgAnter = new MensajesOperacion();

                //validacion de la session del usuario
                //if (string.IsNullOrEmpty(Session["UsuarioAD"].ToString())) return RedirectToAction("Index", "Usuaurio");

                UsuairoModel usuario = new UsuairoModel();

                if (id != null) usuario = _repositorioUsuario.FindId(id);

                if (usuario != null)
                {
                    List<SelectListItem> itemsPerfil = new SelectList(_repositorioUsuario.listaPerfilesActivos(), "idtipouxm", "nombre").ToList();
                    //itemsPerfil.Insert(0, (new SelectListItem { Text = " [Seleccione Perfíl] ", Value = null })); 
                    ViewBag.ListOfPerfiles = new SelectList(itemsPerfil.ToList(), "value", "Text", usuario.idtipousuario);

                    //-- Selector Estado----
                    ViewBag.ListaEstado = new SelectList(
                                   new List<SelectListItem>
                                   {
                                        new SelectListItem { Text = "Activo", Value = "1" },
                                        new SelectListItem { Text = "Inactivo", Value = "0"},
                                   }, "Value", "Text");
                    ///-------------------

                    return View(usuario);
                }
                else
                {
                    mensajes = mensajes.MensajeVista(5, controllerName);
                    mensajes.Mensaje = mensajes.Mensaje + " [ " + actionName + " / " + controllerName + " ] ";

                    TempData["Message"] = mensajes.Mensaje;
                    TempData["AlertType"] = mensajes.TipoMsg;
                    TempData["ShowAlert"] = mensajes.Muestra.ToString();
                    TempData["ShowMsg"] = (mensajes.Mostro.ToString().ToLower().Equals("true")) ? "S" : "N";

                    ViewBag.Message = mensajes.Mensaje;
                    ViewBag.AlertType = mensajes.TipoMsg;
                    ViewBag.ShowAlert = mensajes.Muestra.ToString();
                    ViewBag.ShowMsg = (mensajes.Mostro.ToString().ToLower().Equals("true")) ? "S" : "N";

                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        [HttpPost]
        public ActionResult Edit(UsuairoModel usuario)
        {
            if (verificarSession())
            {
                MensajesOperacion mensajes = new MensajesOperacion();
                int mensajesVista = 0;
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                //validacion de la session del usuario
                if (string.IsNullOrEmpty(Session["UsuarioAD"].ToString())) return RedirectToAction("Index", "Usuaurio");

                List<SelectListItem> itemsPerfil = new SelectList(_repositorioUsuario.listaPerfilesActivos(), "idtipouxm", "nombre").ToList();
                //itemsPerfil.Insert(0, (new SelectListItem { Text = " [Seleccione Perfíl] ", Value = null }));

                ViewBag.ListOfPerfiles = new SelectList(itemsPerfil.ToList(), "value", "Text", usuario.idtipousuario);

                try
                {
                    try
                    {
                        if (TempData["Message"] != null) mensajes.Mensaje = TempData["Message"].ToString();
                        if (TempData["AlertType"] != null) mensajes.TipoMsg = TempData["AlertType"].ToString();
                        if (TempData["ShowAlert"] != null) mensajes.Muestra = (TempData["ShowAlert"].ToString().ToLower().Equals("true")) ? true : false;
                        if (TempData["ShowMsg"] != null) mensajes.Mostro = (TempData["ShowMsg"].ToString().ToLower().Equals("s")) ? true : false;
                    }
                    catch (Exception) { throw; }

                    if (_repositorioUsuario.ValidarCampos(usuario, "edit"))
                    {
                        if (usuario.Password.Length < 64)
                        {
                            usuario.Password = _repositorioUsuario.cifrarContrasena(usuario.Password);
                        }

                        if (_repositorioUsuario.Edit(usuario))
                        {
                            mensajesVista = 6;
                        }
                    }
                    else
                    {
                        mensajesVista = 3;
                    }

                }
                catch (Exception)
                {
                    mensajesVista = 7;
                }

                mensajes = mensajes.MensajeVista(mensajesVista, controllerName);
                mensajes.Mensaje = mensajes.Mensaje + " [ " + actionName + " / " + controllerName + " ] ";

                TempData["Message"] = mensajes.Mensaje;
                TempData["AlertType"] = mensajes.TipoMsg;
                TempData["ShowAlert"] = mensajes.Muestra.ToString();
                TempData["ShowMsg"] = (mensajes.Mostro.ToString().ToLower().Equals("true")) ? "S" : "N";

                ViewBag.Message = mensajes.Mensaje;
                ViewBag.AlertType = mensajes.TipoMsg;
                ViewBag.ShowAlert = mensajes.Muestra.ToString();
                ViewBag.ShowMsg = (mensajes.Mostro.ToString().ToLower().Equals("true")) ? "S" : "N";


                if (mensajes.TipoMsg.Equals("success"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(usuario);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }

            


        }
               
        // GET: TipoUsuarios/Delete/5
        public ActionResult Delete(string id)
        {
            if (verificarSession())
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                MensajesOperacion mensajes = new MensajesOperacion();

                if (id != null)
                {
                    UsuairoModel usuario = new UsuairoModel();
                    List<SelectListItem> itemsPerfil = new SelectList(_repositorioUsuario.listaPerfilesActivos(), "idtipouxm", "nombre").ToList();
                    //itemsPerfil.Insert(0, (new SelectListItem { Text = " [Seleccione Perfíl] ", Value = null })); 
                    ViewBag.ListOfPerfiles = new SelectList(itemsPerfil.ToList(), "value", "Text", usuario.idtipousuario);
                    usuario = _repositorioUsuario.FindId(id);
                    return View(usuario);
                }
                else
                {
                    mensajes = mensajes.MensajeVista(5, controllerName);
                    mensajes.Mensaje = mensajes.Mensaje + " [ " + actionName + " / " + controllerName + " ] ";

                    TempData["Message"] = mensajes.Mensaje;
                    TempData["AlertType"] = mensajes.TipoMsg;
                    TempData["ShowAlert"] = mensajes.Muestra.ToString();
                    TempData["ShowMsg"] = (mensajes.Mostro.ToString().ToLower().Equals("true")) ? "S" : "N";

                    ViewBag.Message = mensajes.Mensaje;
                    ViewBag.AlertType = mensajes.TipoMsg;
                    ViewBag.ShowAlert = mensajes.Muestra.ToString();
                    ViewBag.ShowMsg = (mensajes.Mostro.ToString().ToLower().Equals("true")) ? "S" : "N";

                    return RedirectToAction("Index");
                }

            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }

        }

        // POST: TipoUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(UsuairoModel usuario)
        {
            if (verificarSession())
            {
                MensajesOperacion mensajes = new MensajesOperacion();
                int mensajesVista = 0;
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                try
                {
                    try
                    {
                        if (TempData["Message"] != null) mensajes.Mensaje = TempData["Message"].ToString();
                        if (TempData["AlertType"] != null) mensajes.TipoMsg = TempData["AlertType"].ToString();
                        if (TempData["ShowAlert"] != null) mensajes.Muestra = (TempData["ShowAlert"].ToString().ToLower().Equals("true")) ? true : false;
                        if (TempData["ShowMsg"] != null) mensajes.Mostro = (TempData["ShowMsg"].ToString().ToLower().Equals("s")) ? true : false;
                    }
                    catch (Exception) { throw; }

                    if (_repositorioUsuario.Delete(usuario))
                    {
                        mensajesVista = 9;
                    }
                    else
                    {
                        mensajesVista = 3;
                    }

                }
                catch (Exception)
                {
                    mensajesVista = 7;
                }

                mensajes = mensajes.MensajeVista(mensajesVista, controllerName);
                mensajes.Mensaje = mensajes.Mensaje + " [ " + actionName + " / " + controllerName + " ] ";

                TempData["Message"] = mensajes.Mensaje;
                TempData["AlertType"] = mensajes.TipoMsg;
                TempData["ShowAlert"] = mensajes.Muestra.ToString();
                TempData["ShowMsg"] = (mensajes.Mostro.ToString().ToLower().Equals("true")) ? "S" : "N";

                ViewBag.Message = mensajes.Mensaje;
                ViewBag.AlertType = mensajes.TipoMsg;
                ViewBag.ShowAlert = mensajes.Muestra.ToString();
                ViewBag.ShowMsg = (mensajes.Mostro.ToString().ToLower().Equals("true")) ? "S" : "N";


                if (mensajes.TipoMsg.Equals("success"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(usuario);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        public ActionResult GetPerPopup()
        {
            if (verificarSession())
            {
                string dato = "";
                return View(_repositorioUsuario.getDatosPersona(dato));
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        [HttpPost]
        public ActionResult GetPerPopupConsulta(string registro)
        {
            if (verificarSession())
            {
                return View(_repositorioUsuario.getDatosPersona(registro));
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        private bool verificarSession()
        {
            try
            {
                string usuarioLogueado = "";
                if (!string.IsNullOrEmpty(Session["UsuarioAD"].ToString()))
                {
                    usuarioLogueado = Session["UsuarioAD"].ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}