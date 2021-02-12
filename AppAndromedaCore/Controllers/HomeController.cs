using BAL.Modelos;
using ServiciosApp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AppAndromedaCore.Controllers
{
    public class HomeController : Controller
    {

        private static List<AccesoModel> Access = new List<AccesoModel>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult LogIn()
        {
            return View(new UsuairoModel());
        }

        [HttpPost]
        public ActionResult LogIn(UsuairoModel usuario)
        {
            if (ModelState.IsValid)
            {
                ConsultaPerfilUsuario perfil = new ConsultaPerfilUsuario();
                List<PermisoAccesoModel> accesoModulo;

                //verificación de acceso al modulo
                accesoModulo = perfil.ConsoltaPerModulo(usuario.Login);
                if (accesoModulo.Count > 0)
                {
                    Session["UsuarioID"] = accesoModulo[0].cedula;
                }

                foreach (PermisoAccesoModel registro in accesoModulo)
                {
                    if (registro.nommodulo == "PRINCIPAL")
                    {
                        //CONSULTA DEL MENU DEL USUARIO
                        Access = perfil.ConsultaUsuario(usuario.Login, usuario.Password, registro.nommodulo);
                        if (Access.Count > 0)
                        {
                            if (Access[0].usuario != null)
                            {
                                Session["UsuarioAD"] = usuario.Login.ToString();
                                return RedirectToAction("Dashboard");
                            }
                            else
                            {

                                TempData["Message"] = "Datos incorrectos favor valide el usuario ó contraseña.";
                                TempData["AlertType"] = "warning";
                                ViewBag.Message = "Datos incorrectos favor valide el usuario ó contraseña.";
                                ViewBag.AlertType = "error";
                                return View(usuario);
                            }

                        }
                        else
                        {

                            TempData["Message"] = "Datos incorrectos favor valide el usuario ó contraseña.";
                            TempData["AlertType"] = "warning";
                            ViewBag.Message = "Datos incorrectos favor valide el usuario ó contraseña.";
                            ViewBag.AlertType = "error";
                            return View(usuario);
                        }
                    }
                }
                TempData["Message"] = "No tiene permiso para acceder al aplicativo";
                TempData["AlertType"] = "warning";
                ViewBag.Message = "El usuario no tiene permiso al aplicativo.";
                ViewBag.AlertType = "error";
                return View(usuario);
            }
            else
            {
                return View();
            }
        }

    public ActionResult LogOut()
        {

            Session["UsuarioAD"] = null;

            return RedirectToAction("LogIn");

        }

        //Tablero inicial
        public ActionResult Dashboard(string Usuario)
        {

            if (!string.IsNullOrEmpty(Usuario))
            {
                Session["UsuarioAD"] = Usuario;
                return View();
            }
            else
            {
                if (Session["UsuarioAD"] != null)
                {
                    string usuario = Session["UsuarioAD"].ToString();
                    Session["UsuarioAD"] = usuario;
                    
                    List<AccesoModel> menu = new List<AccesoModel>();
                    foreach (var item in Access)
                    {
                        menu.Add(item);
                    }

                    return View(menu);
                }
                else
                {                   
                     
                    return RedirectToAction("LogIn");
                    
                }
            }
        }
    }

}