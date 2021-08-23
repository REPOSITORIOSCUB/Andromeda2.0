using BAL.Interfaces.Configuracion;
using BAL.Modelos.General;
using BAL.Repositorios.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppAndromedaCore.Controllers.Configuracion
{
    public class UsuarioXTipoUsuarioController : Controller
    {
        IRepositorioUsuarioXTipoUsuairo _iRepositorioUsuarioXTipoUsuairo;

        public UsuarioXTipoUsuarioController()
        {
            if (_iRepositorioUsuarioXTipoUsuairo == null)
            {
                _iRepositorioUsuarioXTipoUsuairo = new RepositorioUsuarioXTipoUsuairo();
            }
        }
        public string usuarioLogueado { get; set; }

        // GET: UsuarioXTipoUsuario
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

                return View(_iRepositorioUsuarioXTipoUsuairo.getobj());
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        // GET: UsuarioXTipoUsuario/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuarioXTipoUsuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioXTipoUsuario/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioXTipoUsuario/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsuarioXTipoUsuario/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioXTipoUsuario/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuarioXTipoUsuario/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private bool verificarSession()
        {
            try
            {
                //temporal
                //Session["UsuarioAD"] = "julian.estrada";

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
