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
    public class ReportesController : Controller
    {
        RepositorioReportes _repositorioReportes;

        public ReportesController()
        {
            if (_repositorioReportes == null)
            {
                _repositorioReportes = new RepositorioReportes();
            }
        }

        public string usuarioLogueado { get; set; }

        // GET: Reportes
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

                //usuarioLogueado = Session["UsuarioAD"].ToString();

                return View(_repositorioReportes.getobj());

            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // GET: Reportes/Create
        public ActionResult Create()
        {
            if (verificarSession())
            {

                ReportesModel data = new ReportesModel();
                //-- Selector  del Modulo
                IEnumerable<ReportesModel> objmodulo = _repositorioReportes.getobj();
                List<SelectListItem> lstgrupo = new List<SelectListItem>();

                var datos = System.Configuration.ConfigurationManager.AppSettings["GrupoReportes"].ToString();

                string[] lstGrupo =  datos.Split('|') ;

                foreach (var grupo in lstGrupo)
                {
                    lstgrupo.Add(new SelectListItem() { Text = grupo, Value = grupo });                    
                }
                lstgrupo.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = null });
                ViewBag.ListaGrupo = new SelectList(lstgrupo.ToList(), "Value", "Text", "");
                //----------------------


                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // POST: Reportes/Create
        [HttpPost]
        public ActionResult Create(ReportesModel reporte)
        {
            if (verificarSession())
            {
                string usuarioLogueado = "";
                if (!string.IsNullOrEmpty(Session["UsuarioAD"].ToString()))
                {
                    usuarioLogueado = Session["UsuarioAD"].ToString();
                };               

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
                    //if (_repositorioModulo.ValidarCampos(modulo, "save"))
                    //{
                    // Este dato deberia tomarse de la session cuando el usuario este logueado                    
                    if (string.IsNullOrEmpty(reporte.UsuarioAdiciona))
                        reporte.UsuarioAdiciona = usuarioLogueado;

                    if (string.IsNullOrEmpty(reporte.FechaRegistro))
                        reporte.FechaRegistro = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// DateTime.Now; // fechPrueba; // 



                    if (_repositorioReportes.Create(reporte))
                        {
                            mensajesVista = 1;
                        }
                        else
                        {
                            mensajesVista = 2;
                        }
                    //}
                    //else
                    //{
                    //    mensajesVista = 3;
                    //}

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
                    return View(reporte);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // GET: Modulos/Edit/5
        public ActionResult Edit(string id)
        {
            if (verificarSession())
            {

                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                MensajesOperacion mensajes = new MensajesOperacion();
                MensajesOperacion msgAnter = new MensajesOperacion();

                ReportesModel tbl = new ReportesModel();

                if (id != null)
                {
                    tbl = _repositorioReportes.FindId(id);
                }

                if (tbl != null)
                {
                    ReportesModel data = new ReportesModel();
                    //-- Selector  del Modulo
                    IEnumerable<ReportesModel> objmodulo = _repositorioReportes.getobj();
                    List<SelectListItem> lstgrupo = new List<SelectListItem>();

                    var datos = System.Configuration.ConfigurationManager.AppSettings["GrupoReportes"].ToString();

                    string[] lstGrupo = datos.Split('|');

                    foreach (var grupo in lstGrupo)
                    {
                        lstgrupo.Add(new SelectListItem() { Text = grupo, Value = grupo });
                    }
                    lstgrupo.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = null });
                    ViewBag.ListaGrupo = new SelectList(lstgrupo.ToList(), "Value", "Text", "");
                    //----------------------
                    //-- Selector Estado----
                    ViewBag.ListaEstado = new SelectList(
                                   new List<SelectListItem>
                                   {
                                        new SelectListItem { Text = "Activo", Value = "1" },
                                        new SelectListItem { Text = "Inactivo", Value = "0"},
                                   }, "Value", "Text", tbl.Activo);
                    ///-------------------


                    if (string.IsNullOrEmpty(tbl.UsuarioAdiciona))
                        tbl.UsuarioAdiciona = usuarioLogueado;
                    if (string.IsNullOrEmpty(tbl.FechaRegistro))
                        tbl.FechaRegistro = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    if (string.IsNullOrEmpty(tbl.UsuarioModifica))
                        tbl.UsuarioModifica = usuarioLogueado;
                    if (string.IsNullOrEmpty(tbl.FechaModificacion))
                        tbl.FechaModificacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

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

        // POST: Modulos/Edit/5
        [HttpPost]
        public ActionResult Edit(ReportesModel reporte)
        {
            if (verificarSession())            {
                

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

                    //if (_repositorioReportes.ValidarCampos(reporte, "edit"))
                    //{
                    if (string.IsNullOrEmpty(reporte.UsuarioModifica))
                        reporte.UsuarioModifica = usuarioLogueado;
                    if (string.IsNullOrEmpty(reporte.FechaModificacion))
                        reporte.FechaModificacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    if (_repositorioReportes.Edit(reporte))
                        {
                            mensajesVista = 6;
                        }
                    //}
                    //else
                    //{
                    //    mensajesVista = 3;
                    //}

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
                    ReportesModel data = new ReportesModel();
                    //-- Selector  del Modulo
                    IEnumerable<ReportesModel> objmodulo = _repositorioReportes.getobj();
                    List<SelectListItem> lstgrupo = new List<SelectListItem>();

                    var datos = System.Configuration.ConfigurationManager.AppSettings["GrupoReportes"].ToString();

                    string[] lstGrupo = datos.Split('|');

                    foreach (var grupo in lstGrupo)
                    {
                        lstgrupo.Add(new SelectListItem() { Text = grupo, Value = grupo });
                    }
                    lstgrupo.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = null });
                    ViewBag.ListaGrupo = new SelectList(lstgrupo.ToList(), "Value", "Text", "");
                    //----------------------
                    //-- Selector Estado----
                    ViewBag.ListaEstado = new SelectList(
                                   new List<SelectListItem>
                                   {
                                        new SelectListItem { Text = "Activo", Value = "1" },
                                        new SelectListItem { Text = "Inactivo", Value = "0"},
                                   }, "Value", "Text", reporte.Activo);
                    ///-------------------
                    return View(reporte);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // GET: Modulos/Reportes/5
        public ActionResult Delete(string id)
        {
            if (verificarSession())
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                MensajesOperacion mensajes = new MensajesOperacion();

                if (id != null)
                {
                    ReportesModel data = new ReportesModel();
                    data = _repositorioReportes.FindId(id);
                    //-- Selector  del Modulo
                    IEnumerable<ReportesModel> objmodulo = _repositorioReportes.getobj();
                    List<SelectListItem> lstgrupo = new List<SelectListItem>();

                    var datos = System.Configuration.ConfigurationManager.AppSettings["GrupoReportes"].ToString();

                    string[] lstGrupo = datos.Split('|');

                    foreach (var grupo in lstGrupo)
                    {
                        lstgrupo.Add(new SelectListItem() { Text = grupo, Value = grupo });
                    }
                    lstgrupo.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = null });
                    ViewBag.ListaGrupo = new SelectList(lstgrupo.ToList(), "Value", "Text", data.Grupo);
                    //----------------------
                    //-- Selector Estado----
                    ViewBag.ListaEstado = new SelectList(
                                   new List<SelectListItem>
                                   {
                                        new SelectListItem { Text = "Activo", Value = "1" },
                                        new SelectListItem { Text = "Inactivo", Value = "0"},
                                   }, "Value", "Text", data.Activo);
                    ///-------------------

                    
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

                    ReportesModel data = new ReportesModel();
                    //-- Selector  del Modulo
                    IEnumerable<ReportesModel> objmodulo = _repositorioReportes.getobj();
                    List<SelectListItem> lstgrupo = new List<SelectListItem>();

                    var datos = System.Configuration.ConfigurationManager.AppSettings["GrupoReportes"].ToString();

                    string[] lstGrupo = datos.Split('|');

                    foreach (var grupo in lstGrupo)
                    {
                        lstgrupo.Add(new SelectListItem() { Text = grupo, Value = grupo });
                    }
                    lstgrupo.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = null });
                    ViewBag.ListaGrupo = new SelectList(lstgrupo.ToList(), "Value", "Text", "");
                    //----------------------
                    //-- Selector Estado----
                    ViewBag.ListaEstado = new SelectList(
                                   new List<SelectListItem>
                                   {
                                        new SelectListItem { Text = "Activo", Value = "1" },
                                        new SelectListItem { Text = "Inactivo", Value = "0"},
                                   }, "Value", "Text","");
                    ///-------------------



                    return RedirectToAction("Index");
                };
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }


        }

        // POST: Modulos/Delete/5
        [HttpPost]
        public ActionResult Delete(ReportesModel reporte)
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

                    if (_repositorioReportes.Delete(reporte))
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
                    return View(reporte);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        //GET : VisorReportes/5
        public ActionResult VisorReportesBI()
        {
            
            #region GRUPOS
            List<Object> lstgrupos = new List<Object>();
            int val = 0;

            var datos = System.Configuration.ConfigurationManager.AppSettings["GrupoReportes"].ToString();

            string[] lstGrupo = datos.Split('|');

            foreach (var grupo in lstGrupo)
            {
                val++;
                lstgrupos.Add(new List<string>[1].Select(item => new List<string> { val.ToString(), grupo }).ToArray());
            }
            ViewBag.ListaGrupos = lstgrupos;
            #endregion

            #region REPORTES
            List<ReportesModel> objreportes = _repositorioReportes.getobj().ToList();

            
            ViewBag.Reportes = objreportes;
            #endregion

            return View();
        }

        /// <summary>
        /// verificación de la session del sistema
        /// </summary>
        /// <returns></returns>
        private bool verificarSession()
        {
            try
            {
                //string usuarioLogueado = "";
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