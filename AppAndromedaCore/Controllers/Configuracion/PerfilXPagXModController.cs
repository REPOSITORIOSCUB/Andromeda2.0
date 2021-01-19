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
    public class PerfilXPagXModController : Controller
    {
        IRepositorioPerfilXPagXMod _repositorioPerfilXPagXMod;
        IRepositorioTipoUsuario _repositorioTipoUsuario;
        IRepositorioPagina _repositorioPagina;
        IRepositorioModulo _repositoriomodulo;

        public PerfilXPagXModController()
        {
            if (_repositorioPerfilXPagXMod == null)
            {
                _repositorioPerfilXPagXMod = new RepositorioPerfilXPapX_Mod();
            }
            if (_repositorioTipoUsuario == null)
            {
                _repositorioTipoUsuario = new RepositorioTipoUsuario();

            }
            if (_repositorioPagina == null)
            {
                _repositorioPagina = new RepositorioPagina();
            }
            if (_repositoriomodulo == null)
            {
                _repositoriomodulo = new RepositorioModulo();
            }
        }

        // GET: PerfilXPagXMod
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

                IEnumerable<PerfilXPagXModModel> respuesta = _repositorioPerfilXPagXMod.getobj();
                TipoUsuarioModel Perfil = new TipoUsuarioModel();
                ModuloModel Mod = new ModuloModel();
                PaginaModel Pag = new PaginaModel();

                try
                {
                    foreach (PerfilXPagXModModel pxm in respuesta)
                    {                       
                        if (pxm.IdPerfil != null)
                        {
                            Perfil = _repositorioTipoUsuario.FindId(pxm.IdPerfil);
                            pxm.IdPerfil = Perfil.Nombre;
                        }
                        if (pxm.IdPagina != null)
                        {
                            Pag = _repositorioPagina.FindId(pxm.IdPagina);

                            pxm.IdPagina = Pag.Mensaje;
                        }
                        if (pxm.IdModulo != null)
                        {
                            Mod = _repositoriomodulo.FindId(pxm.IdModulo);

                            pxm.IdModulo = Mod.Nombre;
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
        
        // GET: PerfilXPagXMod/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PerfilXPagXMod/Create
        public ActionResult Create()
        {
            if (verificarSession())
            {
                PerfilXPagXModModel datos = new PerfilXPagXModModel();

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

                //-- Selector  de Página
                IEnumerable<PaginaModel> objpagina = _repositorioPagina.getobj();
                List<SelectListItem> lstpagina = new List<SelectListItem>();
                foreach (PaginaModel pagina in objpagina)
                {
                    if (pagina.Id.ToString() != null)
                    {
                        lstpagina.Add(new SelectListItem() { Text = pagina.Mensaje, Value = pagina.Id.ToString() });
                    }
                }
                lstpagina.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = null });
                ViewBag.ListaPagina = new SelectList(lstpagina.ToList(), "Value", "Text", "");
                //----------------------

                return View(datos);
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // POST: PerfilXPagXMod/Create
        [HttpPost]
        public ActionResult Create(PerfilXPagXModModel perxpagxmod)
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
                    if (_repositorioPerfilXPagXMod.ValidarCampos(perxpagxmod, "save"))
                    {
                        if (_repositorioPerfilXPagXMod.Create(perxpagxmod))
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
                    return View(perxpagxmod);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // GET: PerfilXPagXMod/Edit/5
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

                PerfilXPagXModModel objperxpagxmod = new PerfilXPagXModModel();

                if (id != null) objperxpagxmod = _repositorioPerfilXPagXMod.FindId(id);

                if (objperxpagxmod != null)
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
                    ViewBag.ListaPerfil = new SelectList(lstperfil.ToList(), "Value", "Text", objperxpagxmod.IdPerfil);
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
                    ViewBag.ListaModulo = new SelectList(lstmodulo.ToList(), "Value", "Text", objperxpagxmod.IdModulo);
                    //----------------------
                    //-- Selector  de Página
                    IEnumerable<PaginaModel> objpagina = _repositorioPagina.getobj();
                    List<SelectListItem> lstpagina = new List<SelectListItem>();
                    foreach (PaginaModel pagina in objpagina)
                    {
                        if (pagina.Id.ToString() != null)
                        {
                            lstpagina.Add(new SelectListItem() { Text = pagina.Mensaje, Value = pagina.Id.ToString() });
                        }
                    }
                    ViewBag.ListaPagina = new SelectList(lstpagina.ToList(), "Value", "Text", objperxpagxmod.IdPagina);
                    //----------------------
                    //-- Selector Estado----
                    ViewBag.ListaEstado = new SelectList(
                                   new List<SelectListItem>
                                   {
                                        new SelectListItem { Text = "Activo", Value = "1" },
                                        new SelectListItem { Text = "Inactivo", Value = "0"},
                                   }, "Value", "Text");
                    ///-------------------
                    return View(objperxpagxmod);
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

        // POST: PerfilXPagXMod/Edit/5
        [HttpPost]
        public ActionResult Edit(PerfilXPagXModModel perfilXpagXmod)
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

                    if (_repositorioPerfilXPagXMod.ValidarCampos(perfilXpagXmod, "edit"))
                    {
                        if (_repositorioPerfilXPagXMod.Edit(perfilXpagXmod))
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
                    return View(perfilXpagXmod);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // GET: PerfilXPagXMod/Delete/5
        public ActionResult Delete(string  id)
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

                PerfilXPagXModModel objperxpagxmod = new PerfilXPagXModModel();

                if (id != null) objperxpagxmod = _repositorioPerfilXPagXMod.FindId(id);

                if (objperxpagxmod != null)
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
                    ViewBag.ListaPerfil = new SelectList(lstperfil.ToList(), "Value", "Text", objperxpagxmod.IdPerfil);
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
                    ViewBag.ListaModulo = new SelectList(lstmodulo.ToList(), "Value", "Text", objperxpagxmod.IdModulo);
                    //----------------------
                    //-- Selector  de Página
                    IEnumerable<PaginaModel> objpagina = _repositorioPagina.getobj();
                    List<SelectListItem> lstpagina = new List<SelectListItem>();
                    foreach (PaginaModel pagina in objpagina)
                    {
                        if (pagina.Id.ToString() != null)
                        {
                            lstpagina.Add(new SelectListItem() { Text = pagina.Mensaje, Value = pagina.Id.ToString() });
                        }
                    }
                    ViewBag.ListaPagina = new SelectList(lstpagina.ToList(), "Value", "Text", objperxpagxmod.IdPagina);
                    //----------------------
                    //-- Selector Estado----
                    ViewBag.ListaEstado = new SelectList(
                                   new List<SelectListItem>
                                   {
                                        new SelectListItem { Text = "Activo", Value = "1" },
                                        new SelectListItem { Text = "Inactivo", Value = "0"},
                                   }, "Value", "Text");
                    ///-------------------


                    return View(objperxpagxmod);
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

        // POST: PerfilXPagXMod/Delete/5
        [HttpPost]
        public ActionResult Delete(PerfilXPagXModModel perfilXpagXmod )
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

                    if (_repositorioPerfilXPagXMod.Delete(perfilXpagXmod))
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
                    return View(perfilXpagXmod);
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
