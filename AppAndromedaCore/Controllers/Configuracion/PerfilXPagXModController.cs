using BAL.Interfaces.Configuracion;
using BAL.Modelos.Configuracion;
using BAL.Modelos.General;
using BAL.Repositorios.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                lstmodulo.Insert(0, new SelectListItem { Text = "", Value = null });
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
                //lstpagina.Insert(0, new SelectListItem { Text = "", Value = null });
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


                    List<PerfilXPagXModModel> ListaPPM = _repositorioPerfilXPagXMod.getobj().ToList();
                    //IEnumerable<PerfilXPagXModModel> paginas = from PerfilXPagXModModel in ListaPPM where PerfilXPagXModModel.IdPerfil == "54" orderby PerfilXPagXModModel.IdPerfil select PerfilXPagXModModel;
                    //IEnumerable<PerfilXPagXModModel> paginas = ListaPPM.Where(x => x.IdPerfil == perxpagxmod.IdPerfil);
                    //IEnumerable<PerfilXPagXModModel> 
                    var paginas = from listapp in ListaPPM
                                  where listapp.IdPerfil.All(Idp => Idp.ToString() == perxpagxmod.IdPerfil.ToString())
                                  select listapp.IdPagina;
                    //var paginas = from listapp in ListaPPM
                    //              where listapp.IdPerfil.Any(Idp => Idp.ToString() == perxpagxmod.IdPerfil.ToString())
                    //              select listapp.IdPagina;
                    if (paginas.Count() > 0)
                    {
                        for (int j = 0; j < perxpagxmod.IdPaginaN.Count(); j++)
                         {                      

                            for (int i = 0; i < paginas.Count(); i++)
                            {
                                var pxpxm = new PerfilXPagXModModel
                                {
                                    Id = perxpagxmod.Id,
                                    IdPagina = perxpagxmod.IdPaginaN[j],
                                    IdModulo = perxpagxmod.IdModulo,
                                    IdPerfil = perxpagxmod.IdPerfil,
                                    Estado = perxpagxmod.Estado,
                                };

                                if (pxpxm.IdPagina == "33021")
                                {
                               
                                    _repositorioPerfilXPagXMod.Delete(pxpxm);

                                }
                                else
                                {
                                    if (true)
                                    {

                                    }
                                    
                                }
                            }
                        }

                    }
                    else
                    {
                        bool flag = true;
                        for (int j = 0; j < perxpagxmod.IdPaginaN.Count(); j++)
                        {
                            var pxpxm = new PerfilXPagXModModel
                            {
                                Id = perxpagxmod.Id,
                                IdPagina = perxpagxmod.IdPaginaN[j],
                                IdModulo = perxpagxmod.IdModulo,
                                IdPerfil = perxpagxmod.IdPerfil,
                                Estado = perxpagxmod.Estado,
                            };

                            if (_repositorioPerfilXPagXMod.ValidarCampos(pxpxm, "save"))
                            {
                                if (_repositorioPerfilXPagXMod.Create(perxpagxmod))
                                {
                                    //mensajesVista = 1;
                                }
                                else
                                {
                                    flag = false;///mensajesVista = 2;
                                }
                            }                            
                        }

                        if (flag) {

                            mensajesVista = 1;
                        }
                        else
                        {
                            mensajesVista = 2;
                        }
                        
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
        public ActionResult Delete(PerfilXPagXModModel perfilXpagXmod)
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
        public async Task<JsonResult> GetPagina(string pxm)
        {

            if (pxm == null)
                return Json(new { success = false, Message = "Error en la consulta" }, JsonRequestBehavior.AllowGet);
            else
            {

                IEnumerable<ModuloModel> listaM = _repositoriomodulo.getobj();
                var modulo = listaM.Where(x => x.Id == pxm).Single();

                IEnumerable<PerfilXPagXModModel> ListaPPM = _repositorioPerfilXPagXMod.getobj();
                var pagina = ListaPPM.Where(x => x.IdModulo == modulo.Id).ToList();


                var idPagina1 = pagina.Select(x => new { x.IdPagina }).Distinct().ToList();
                var idPerfil = pagina.Select(x => new { x.IdPerfil }).Distinct().ToList();
                foreach (var item in pagina)
                {
                    if (item.IdPagina != null)
                    {
                        var p = _repositorioPagina.FindId(item.IdPagina);
                        var perf = _repositorioTipoUsuario.FindId(item.IdPerfil);
                        item.IdPerfil = perf.Nombre;
                        item.IdPagina = p.Mensaje;
                    }
                }

                var lista = pagina.Select(x => new { x.IdPagina }).Distinct().ToList();
                var Perfil = pagina.Select(x => x.IdPerfil).Distinct().ToList();
                var datos = new { lista, idPagina1, Perfil, idPerfil };

                return Json(new { success = true, datos }, JsonRequestBehavior.AllowGet);

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
