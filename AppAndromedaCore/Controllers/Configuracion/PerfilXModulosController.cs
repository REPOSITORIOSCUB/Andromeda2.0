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
    public class PerfilXModulosController : Controller
    {
        IRepositorioPerfilXModulo _repositorioPerfilXModulo;
        IRepositorioModulo _repositoriomodulo;
        IRepositorioTipoUsuario _repositorioTipoUsuario;

        public PerfilXModulosController()
        {
            if (_repositorioPerfilXModulo == null)
            {
                _repositorioPerfilXModulo = new RepositorioPerfilXModulo();
            }

            if (_repositorioTipoUsuario == null)
            {
                _repositorioTipoUsuario = new RepositorioTipoUsuario();
            }
            if (_repositoriomodulo == null)
            {
                _repositoriomodulo = new RepositorioModulo();
            }
        }

        // GET: PerfilXModulot
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

                IEnumerable<PerfilXModuloModel> respuesta = _repositorioPerfilXModulo.getobj();
                TipoUsuarioModel Perfil = new TipoUsuarioModel();
                ModuloModel Mod = new ModuloModel();

                try
                {
                    foreach (PerfilXModuloModel pxm in respuesta)
                    {
                        if (pxm.IdModulo != null)
                        {
                            Mod = _repositoriomodulo.FindId(pxm.IdModulo);

                            pxm.IdModulo = Mod.Nombre;
                        }
                        if (pxm.IdPerfil != null)
                        {
                            Perfil = _repositorioTipoUsuario.FindId(pxm.IdPerfil);
                            pxm.IdPerfil = Perfil.Nombre;
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

        // GET: PerfilXModulot/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PerfilXModulot/Create
        public ActionResult Create()
        {
            if (verificarSession())
            {
                PerfilXModuloModel datos = new PerfilXModuloModel();
                              
                //----Selector de perfil
                IEnumerable<TipoUsuarioModel> objperfil = _repositorioTipoUsuario.getTipoUsuairoSinAsignar();
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
                IEnumerable<ModuloModel> objmodulo = _repositoriomodulo.getobj();
                List<SelectListItem> lstmodulo = new List<SelectListItem>();
                foreach (ModuloModel modulo in objmodulo)
                {
                    if (modulo.Id != null)
                    {
                        lstmodulo.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Id });
                    }
                }
                lstmodulo.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = null });
                ViewBag.ListaModulo = new SelectList(lstmodulo.ToList(), "Value", "Text", "");
                //----------------------

                return View(datos);
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // POST: PerfilXModulot/Create
        [HttpPost]
        public ActionResult Create(PerfilXModuloModel perxmod)
        {
            if(verificarSession())
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
                    if (_repositorioPerfilXModulo.ValidarCampos(perxmod, "save"))
                    {
                        if (_repositorioPerfilXModulo.Create(perxmod))
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
                    return View(perxmod);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // GET: PerfilXModulot/Edit/5
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

                PerfilXModuloModel objperxmod = new PerfilXModuloModel();

                if (id != null) objperxmod = _repositorioPerfilXModulo.FindId(id);

                if (objperxmod != null)
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
                    ViewBag.ListaPerfil = new SelectList(lstperfil.ToList(), "Value", "Text", objperxmod.IdPerfil);
                    //----------------------
                    //-- Selector  del Modulo
                    IEnumerable<ModuloModel> objmodulo = _repositoriomodulo.getobj();
                    List<SelectListItem> lstmodulo = new List<SelectListItem>();
                    foreach (ModuloModel modulo in objmodulo)
                    {
                        if (modulo.Id != null)
                        {
                            lstmodulo.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Id });
                        }
                    }
                    ViewBag.ListaModulo = new SelectList(lstmodulo.ToList(), "Value", "Text", objperxmod.IdModulo);
                    //----------------------
                    //-- Selector Estado----
                    ViewBag.ListaEstado = new SelectList(
                                   new List<SelectListItem>
                                   {
                                        new SelectListItem { Text = "Activo", Value = "1" },
                                        new SelectListItem { Text = "Inactivo", Value = "0"}, 
                                   }, "Value", "Text");
                    ///-------------------
                    return View(objperxmod);
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

        // POST: PerfilXModulot/Edit/5
        [HttpPost]
        public ActionResult Edit(PerfilXModuloModel perxmod)
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

                    if (_repositorioPerfilXModulo.ValidarCampos(perxmod, "edit"))
                    {
                        if (_repositorioPerfilXModulo.Edit(perxmod))
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
                    return View(perxmod);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // GET: PerfilXModulot/Delete/5
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

                PerfilXModuloModel objperxmod = new PerfilXModuloModel();

                if (id != null) objperxmod = _repositorioPerfilXModulo.FindId(id);

                if (objperxmod != null)
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
                    ViewBag.ListaPerfil = new SelectList(lstperfil.ToList(), "Value", "Text", objperxmod.IdPerfil);
                    //----------------------
                    //-- Selector  del Modulo
                    IEnumerable<ModuloModel> objmodulo = _repositoriomodulo.getobj();
                    List<SelectListItem> lstmodulo = new List<SelectListItem>();
                    foreach (ModuloModel modulo in objmodulo)
                    {
                        if (modulo.Id != null)
                        {
                            lstmodulo.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Id });
                        }
                    }
                    ViewBag.ListaModulo = new SelectList(lstmodulo.ToList(), "Value", "Text", objperxmod.IdModulo);
                    //----------------------
                    //-- Selector Estado----
                    ViewBag.ListaEstado = new SelectList(
                                   new List<SelectListItem>
                                   {
                                        new SelectListItem { Text = "Activo", Value = "1" },
                                        new SelectListItem { Text = "Inactivo", Value = "0"},
                                   }, "Value", "Text");
                    ///-------------------


                    return View(objperxmod);
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

        // POST: PerfilXModulot/Delete/5
        [HttpPost]
        public ActionResult Delete(PerfilXModuloModel perxmod)
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

                    if (_repositorioPerfilXModulo.Delete(perxmod))
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
                    return View(perxmod);
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
