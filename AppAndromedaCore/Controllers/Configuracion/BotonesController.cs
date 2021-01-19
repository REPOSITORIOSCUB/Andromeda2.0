using BAL.Interfaces.Configuracion;
using BAL.Modelos.Configuracion;
using BAL.Modelos.General;
using BAL.Repositorios.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppAndromedaCore.Controllers.Configuracion
{
    public class BotonesController : Controller
    {
        IRepositorioBoton _repositorioBoton;

        public BotonesController()
        {
            if (_repositorioBoton == null)
            {
                _repositorioBoton = new RepositorioBoton();
            }    
        }
        // GET: Botones
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

                return View(_repositorioBoton.getobj());
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

       

        // GET: Botones/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Botones/Create
        public ActionResult Create()
        {
            if (verificarSession())
            {
                BotonesModel data = new BotonesModel();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // POST: Botones/Create
        [HttpPost]
        public ActionResult Create(BotonesModel boton)
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
                    if (_repositorioBoton.ValidarCampos(boton, "save"))
                    {
                        if (_repositorioBoton.Create(boton))
                        {
                            mensajesVista = 1;
                        }
                        else
                        {
                            mensajesVista = 2;
                        }
                    }
                    else
                    {
                        mensajesVista = 3;
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
                    return View(boton);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // GET: Botones/Edit/5
        public ActionResult Edit(string id)
        {
            if (verificarSession())
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                MensajesOperacion mensajes = new MensajesOperacion();
                MensajesOperacion msgAnter = new MensajesOperacion();

                BotonesModel tbl = new BotonesModel();

                if (id != null)
                {
                    tbl = _repositorioBoton.FindId(id);
                }

                if (tbl != null)
                {
                    return View(tbl);
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

        // POST: Botones/Edit/5
        [HttpPost]
        public ActionResult Edit(BotonesModel boton)
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

                    if (_repositorioBoton.ValidarCampos(boton, "edit"))
                    {
                        if (_repositorioBoton.Edit(boton))
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
                    return View(boton);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // GET: Botones/Delete/5
        public ActionResult Delete(string id)
        {
            if (verificarSession())
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                MensajesOperacion mensajes = new MensajesOperacion();

                if (id != null)
                {
                    BotonesModel data = new BotonesModel();

                    data = _repositorioBoton.FindId(id);
                    return View(data);
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
                };
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // POST: Botones/Delete/5
        [HttpPost]
        public ActionResult Delete(BotonesModel boton)
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

                    if (_repositorioBoton.Delete(boton))
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
                    return View(boton);
                }
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
