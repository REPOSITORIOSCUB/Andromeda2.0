using BAL.Interfaces.Configuracion;
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
    public class BotonXPerfilController : Controller
    {
        IRepositorioBotonXPerXPag _repositorioBotonXPer;
        IRepositorioBoton _repositorioBoton;
        IRepositorioTipoUsuario _repositorioTipoUsuario;

        public BotonXPerfilController()
        {
            if (_repositorioBotonXPer == null)
            {
                _repositorioBotonXPer = new RepositorioBotonxPerXPag();
            }
            if (_repositorioBoton == null)
            {
                _repositorioBoton = new RepositorioBoton();
            }
            if (_repositorioTipoUsuario == null)
            {
                _repositorioTipoUsuario = new RepositorioTipoUsuario();
            }
             
        }

        // GET: BotonXPerfil
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

                IEnumerable<BotonXPerXPagModel> respuesta = _repositorioBotonXPer.getobj();
                TipoUsuarioModel Perfil = new TipoUsuarioModel();
                BotonesModel boton = new BotonesModel();
              

                try
                {
                    foreach (BotonXPerXPagModel pxm in respuesta)
                    {
                        if (pxm.IdPerfil != null)
                        {
                            Perfil = _repositorioTipoUsuario.FindId(pxm.IdPerfil);
                            pxm.IdPerfil = Perfil.Nombre;
                        }
                        if (pxm.IdBoton != null)
                        {
                            boton = _repositorioBoton.FindId(pxm.IdBoton);

                            pxm.IdBoton = boton.Nombre;
                        }
                       
                    }
                }
                catch (Exception err)
                {

                    throw;
                }

                //return View(_repositorioMenu.getobj());
                return View(respuesta);
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        

        // GET: BotonXPerfil/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BotonXPerfil/Create
        public ActionResult Create()
        {
            if (verificarSession())
            {
                BotonXPerXPagModel datos = new BotonXPerXPagModel();

                //----Selector de perfil
                IEnumerable<TipoUsuarioModel> objperfil = _repositorioTipoUsuario.getobj();
                List<SelectListItem> lstperfil = new List<SelectListItem>();
                foreach (TipoUsuarioModel perfil in objperfil)
                {
                    if (!String.IsNullOrEmpty(perfil.Id.ToString()))
                    {
                        lstperfil.Add(new SelectListItem() { Text = perfil.Nombre, Value = perfil.Id.ToString() });
                    }
                }
                lstperfil.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = null });
                ViewBag.ListaPerfil = new SelectList(lstperfil.ToList(), "Value", "Text", "");
                //----------------------
                //-- Selector  del Modulo
                IEnumerable<BotonesModel> objboton = _repositorioBoton.getobj();
                List<SelectListItem> lstboton = new List<SelectListItem>();
                foreach (BotonesModel modulo in objboton)
                {
                    if (modulo.Id != null)
                    {
                        lstboton.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Id });
                    }
                }
                lstboton.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = null });
                ViewBag.ListaBoton = new SelectList(lstboton.ToList(), "Value", "Text", "");
                //----------------------                

                return View(datos);
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // POST: BotonXPerfil/Create
        [HttpPost]
        public ActionResult Create(BotonXPerXPagModel botonXPer)
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
                    if (_repositorioBotonXPer.ValidarCampos(botonXPer, "save"))
                    {
                        if (_repositorioBotonXPer.Create(botonXPer))
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
                    return View(botonXPer);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // GET: BotonXPerfil/Edit/5
        public ActionResult Edit(string id)
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

                BotonXPerXPagModel objbotonxper = new BotonXPerXPagModel();

                if (id != null) objbotonxper = _repositorioBotonXPer.FindId(id);

                if (objbotonxper != null)
                {

                    //----Selector de perfil
                    IEnumerable<TipoUsuarioModel> objperfil = _repositorioTipoUsuario.getobj();
                    List<SelectListItem> lstperfil = new List<SelectListItem>();
                    foreach (TipoUsuarioModel perfil in objperfil)
                    {
                        if (!String.IsNullOrEmpty(perfil.Id.ToString()))
                        {
                            lstperfil.Add(new SelectListItem() { Text = perfil.Nombre, Value = perfil.Id.ToString() });
                        }
                    }
                    ViewBag.ListaPerfil = new SelectList(lstperfil.ToList(), "Value", "Text", objbotonxper.IdPerfil);
                    //----------------------
                    //-- Selector  del boton
                    IEnumerable<BotonesModel> objboton = _repositorioBoton.getobj();
                    List<SelectListItem> lstboton = new List<SelectListItem>();
                    foreach (BotonesModel modulo in objboton)
                    {
                        if (modulo.Id != null)
                        {
                            lstboton.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Id });
                        }
                    }
                    ViewBag.ListaBoton = new SelectList(lstboton.ToList(), "Value", "Text", objbotonxper.IdBoton);
                    //-- Selector Estado----
                    ViewBag.ListaEstado = new SelectList(
                                   new List<SelectListItem>
                                   {
                                        new SelectListItem { Text = "Activo", Value = "1" },
                                        new SelectListItem { Text = "Inactivo", Value = "0"},
                                   }, "Value", "Text");
                    ///-------------------
                    return View(objbotonxper);
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

        // POST: BotonXPerfil/Edit/5
        [HttpPost]
        public ActionResult Edit(BotonXPerXPagModel botonXPer)
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

                    if (_repositorioBotonXPer.ValidarCampos(botonXPer, "edit"))
                    {
                        if (_repositorioBotonXPer.Edit(botonXPer))
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
                    return View(botonXPer);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // GET: BotonXPerfil/Delete/5
        public ActionResult Delete(string id)
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

                BotonXPerXPagModel objbotonxper = new BotonXPerXPagModel();

                if (id != null) objbotonxper = _repositorioBotonXPer.FindId(id);

                if (objbotonxper != null)
                {
                    //----Selector de perfil
                    IEnumerable<TipoUsuarioModel> objperfil = _repositorioTipoUsuario.getobj();
                    List<SelectListItem> lstperfil = new List<SelectListItem>();
                    foreach (TipoUsuarioModel perfil in objperfil)
                    {
                        if (!String.IsNullOrEmpty(perfil.Id.ToString()))
                        {
                            lstperfil.Add(new SelectListItem() { Text = perfil.Nombre, Value = perfil.Id.ToString() });
                        }
                    }
                    ViewBag.ListaPerfil = new SelectList(lstperfil.ToList(), "Value", "Text", objbotonxper.IdPerfil);
                    //-- Selector  del boton
                    IEnumerable<BotonesModel> objboton = _repositorioBoton.getobj();
                    List<SelectListItem> lstboton = new List<SelectListItem>();
                    foreach (BotonesModel modulo in objboton)
                    {
                        if (modulo.Id != null)
                        {
                            lstboton.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Id });
                        }
                    }
                    ViewBag.ListaBoton = new SelectList(lstboton.ToList(), "Value", "Text", objbotonxper.IdBoton);
                    //----------------------
                    //-- Selector Estado----
                    ViewBag.ListaEstado = new SelectList(
                                   new List<SelectListItem>
                                   {
                                        new SelectListItem { Text = "Activo", Value = "1" },
                                        new SelectListItem { Text = "Inactivo", Value = "0"},
                                   }, "Value", "Text");
                    ///-------------------


                    return View(objbotonxper);
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

        // POST: BotonXPerfil/Delete/5
        [HttpPost]
        public ActionResult Delete(BotonXPerXPagModel botonXPer)
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

                    if (_repositorioBotonXPer.Delete(botonXPer))
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
                    return View(botonXPer);
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
