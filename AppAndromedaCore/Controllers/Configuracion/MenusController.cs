using BAL.Interfaces.Configuracion;
using BAL.Modelos.Configuracion;
using BAL.Modelos.General;
using BAL.Repositorios.Configuracion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppAndromedaCore.Controllers.Configuracion
{
    public class MenusController : Controller
    {
        IRepositorioMenu _repositorioMenu;
        IRepositorioPagina _repositorioPagina;
        IRepositorioModulo _repositoriomodulo;

        public MenusController()
        {
            if (_repositorioMenu == null)
            {
                _repositorioMenu = new RepositorioMenu();
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
        // GET: Menus
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

                IEnumerable<MenuModel> respuesta = _repositorioMenu.getobj();
                PaginaModel Pagina = new PaginaModel();
                ModuloModel Mod = new ModuloModel();

                try
                {
                    foreach (MenuModel menu in respuesta)
                    {
                        if (menu.IdPagina != null)
                        {
                            Pagina = _repositorioPagina.FindId(menu.IdPagina);

                            menu.IdPagina = Pagina.Mensaje;
                        }
                        if (menu.IdModulo != null)
                        {
                            Mod = _repositoriomodulo.FindId(menu.IdModulo);
                            menu.IdModulo = Mod.Nombre;
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

        // GET: Menus/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Menus/Create
        public ActionResult Create()
        {
            if (verificarSession())
            {
                MenuModel datos = new MenuModel();
                //-----Selector Menu Padre
                IEnumerable<MenuModel> respuesta = _repositorioMenu.getobj();                
                List<SelectListItem> lstmenu = new List<SelectListItem>();
                foreach (MenuModel menu in respuesta)
                {
                    if (menu.Id != null)
                    {
                        lstmenu.Add(new SelectListItem() { Text = menu.Nombre, Value = menu.Id });
                    }    
                }
                lstmenu.Insert(0, new SelectListItem { Text="--Seleccione--", Value= null });
                ViewBag.ListaMenuPadre = new SelectList(lstmenu.ToList(),"Value", "Text","");
                //-----------------------
                //----Selector de Pagina
                IEnumerable<PaginaModel> objpaginas = _repositorioPagina.getobj();
                List<SelectListItem> lstpagina = new List<SelectListItem>();
                foreach (PaginaModel pagina in objpaginas)
                {
                    if (!String.IsNullOrEmpty(pagina.Id.ToString()))
                    {
                        lstpagina.Add( new SelectListItem() { Text= pagina.Mensaje,Value = pagina.Id.ToString()});
                    }
                }
                lstpagina.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = null });
                ViewBag.ListaPagina = new SelectList(lstpagina.ToList(),"Value", "Text","");
                //----------------------
                //-- Selector  del Modulo
                IEnumerable<ModuloModel> objmodulo = _repositoriomodulo.getobj();
                List<SelectListItem> lstmodulo = new List<SelectListItem>();
                foreach (ModuloModel modulo in objmodulo)
                {
                    if (modulo.Id != null)
                    {
                        lstmodulo.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Id});
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

        // POST: Menus/Create
        [HttpPost]
        public ActionResult Create(MenuModel menu)
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
                    if (_repositorioMenu.ValidarCampos(menu, "save"))
                    {
                        if (_repositorioMenu.Create(menu))
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
                    return View(menu);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // GET: Menus/Edit/5
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

                MenuModel objmenu = new MenuModel();

                if (id != null) objmenu = _repositorioMenu.FindId(id);

                if (objmenu != null)
                {                    
                    //-----Selector Menu Padre
                    IEnumerable<MenuModel> respuesta = _repositorioMenu.getobj();
                    List<SelectListItem> lstmenu = new List<SelectListItem>();
                    foreach (MenuModel menu in respuesta)
                    {
                        if (menu.Id != null)
                        {
                            lstmenu.Add(new SelectListItem() { Text = menu.Nombre, Value = menu.Id });
                        }
                    }
                    lstmenu.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = null });
                    ViewBag.ListaMenuPadre = new SelectList(lstmenu.ToList(), "Value", "Text", objmenu.IdPadre);
                    //-----------------------
                    //----Selector de Pagina
                    IEnumerable<PaginaModel> objpaginas = _repositorioPagina.getobj();
                    List<SelectListItem> lstpagina = new List<SelectListItem>();
                    foreach (PaginaModel pagina in objpaginas)
                    {
                        if (!String.IsNullOrEmpty(pagina.Id.ToString()))
                        {
                            lstpagina.Add(new SelectListItem() { Text = pagina.Mensaje, Value = pagina.Id.ToString() });
                        }
                    }
                    lstpagina.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = null });
                    ViewBag.ListaPagina = new SelectList(lstpagina.ToList(), "Value", "Text", objmenu.IdPagina);
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
                    ViewBag.ListaModulo = new SelectList(lstmodulo.ToList(), "Value", "Text", objmenu.IdModulo);
                    //----------------------


                    return View(objmenu);
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

        // POST: Menus/Edit/5
        [HttpPost]
        public ActionResult Edit(MenuModel menu)
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

                    if (_repositorioMenu.ValidarCampos(menu, "edit"))
                    {
                        if (_repositorioMenu.Edit(menu))
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
                    return View(menu);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // GET: Menus/Delete/5
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

                MenuModel objmenu = new MenuModel();

                if (id != null) objmenu = _repositorioMenu.FindId(id);

                if (objmenu != null)
                {
                    //-----Selector Menu Padre
                    IEnumerable<MenuModel> respuesta = _repositorioMenu.getobj();
                    List<SelectListItem> lstmenu = new List<SelectListItem>();
                    foreach (MenuModel menu in respuesta)
                    {
                        if (menu.Id != null)
                        {
                            lstmenu.Add(new SelectListItem() { Text = menu.Nombre, Value = menu.Id });
                        }
                    }
                    ViewBag.ListaMenuPadre = new SelectList(lstmenu.ToList(), "Value", "Text", objmenu.IdPadre);
                    //-----------------------
                    //----Selector de Pagina
                    IEnumerable<PaginaModel> objpaginas = _repositorioPagina.getobj();
                    List<SelectListItem> lstpagina = new List<SelectListItem>();
                    foreach (PaginaModel pagina in objpaginas)
                    {
                        if (!String.IsNullOrEmpty(pagina.Id.ToString()))
                        {
                            lstpagina.Add(new SelectListItem() { Text = pagina.Mensaje, Value = pagina.Id.ToString() });
                        }
                    }
                    ViewBag.ListaPagina = new SelectList(lstpagina.ToList(), "Value", "Text", objmenu.IdPagina);
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
                    ViewBag.ListaModulo = new SelectList(lstmodulo.ToList(), "Value", "Text", objmenu.IdModulo);
                    //----------------------


                    return View(objmenu);
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

        // POST: Menus/Delete/5
        [HttpPost]
        public ActionResult Delete(MenuModel menu)
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

                    if (_repositorioMenu.Delete(menu))
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
                    return View(menu);
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
