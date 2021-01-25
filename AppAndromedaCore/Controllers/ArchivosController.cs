using BAL.Modelos.General;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppAndromedaCore.Controllers
{
    public class ArchivosController : Controller
    {
        private static string opcion ,identificador;
        // GET: Archivos
        [HttpGet]
        public ActionResult Index(string id ,string accion)
        {
            //List<Archivo> _archivos = new List<Archivo>();
            //using (_dbContext = new AppDbContext())
            //{
            //    // Recuperamos la Lista de los archivos subidos.
            //    _archivos = _dbContext.Archivos.OrderBy(x => x.Creado).ToList();
            //}
            // Retornamos la Vista Index, con los archivos subidos.
            //return View(_archivos);
            opcion = accion;
            identificador = id;
            if (TempData["Message"] != null)
            {
                ViewBag.Mensjae = TempData["Message"].ToString();
            }

            return View();
        }


        [HttpPost]
        public ActionResult SubirArchivo(ArchivoModel archivo)
        {
            string RutaSitio = Server.MapPath("~/");
            string Carpeta = ConfigurationManager.AppSettings["RutaImagenes"].ToString();

            if (!ModelState.IsValid)
            {
                return View("Index", archivo);
            }
            string rutaArchivo = Path.Combine(RutaSitio + Carpeta);
            if (!Directory.Exists(rutaArchivo))
            {
                Directory.CreateDirectory(rutaArchivo);
            }
            rutaArchivo = rutaArchivo + archivo.ruta.FileName;
            
            if (!System.IO.File.Exists(rutaArchivo))
            {                
                archivo.ruta.SaveAs(rutaArchivo);
            }
           
            @TempData["NombreImagen"] = archivo.ruta.FileName;
            @TempData["RutaImagen"] ="/"+ Carpeta.Replace('\\', '/') + archivo.ruta.FileName;

            if (opcion == "Create")
            {
               return RedirectToAction("Create", "Menus");
            }
            else
            {
                return RedirectToAction("Edit", "Menus", new { id = identificador });
            }
           
            //return View();
        }

        [HttpPost]
        public ActionResult SubirArchivo2 (ArchivoModel archivo)
        {
            string RutaSitio = Server.MapPath("~/");
            string Carpeta = ConfigurationManager.AppSettings["RutaImagenes"].ToString();

            if (!ModelState.IsValid)
            {
                return View("Index", archivo);
            }
            string rutaArchivo = Path.Combine(RutaSitio + Carpeta);
            if (!Directory.Exists(rutaArchivo))
            {
                Directory.CreateDirectory(rutaArchivo);
            }
            rutaArchivo = rutaArchivo + archivo.ruta.FileName;

            if (!System.IO.File.Exists(rutaArchivo))
            {
                archivo.ruta.SaveAs(rutaArchivo);
            }

            @TempData["NombreImagen"] = archivo.ruta.FileName;
            @TempData["RutaImagen"] = "/" + Carpeta.Replace('\\', '/') + archivo.ruta.FileName;

            return RedirectToAction("Edit", "Menus");
            //return View();
        }


        //[HttpPost]
        //public ActionResult SubirArchivo(HttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        // Extraemos el contenido en Bytes del archivo subido.
        //        var _contenido = new byte[file.ContentLength];
        //        file.InputStream.Read(_contenido, 0, file.ContentLength);

        //        // Separamos el Nombre del archivo de la Extensión.
        //        int indiceDelUltimoPunto = file.FileName.LastIndexOf('.');
        //        string _nombre = file.FileName.Substring(0, indiceDelUltimoPunto);
        //        string _extension = file.FileName.Substring(indiceDelUltimoPunto + 1,
        //                            file.FileName.Length - indiceDelUltimoPunto - 1);

        //        // Instanciamos la clase Archivo y asignammos los valores.
        //        Archivo _archivo = new Archivo()
        //        {
        //            Nombre = _nombre,
        //            Extension = _extension,
        //           // Descargas = 0
        //        };

        //        try
        //        {
        //            // Subimos el archivo al Servidor.
        //            _archivo.SubirArchivo(_contenido);

        //            ViewBag.Archivo = _archivo;
        //            // Guardamos en la base de datos la instancia del archivo
        //            //using (_dbContext = new AppDbContext())
        //            //{
        //            //    _dbContext.Archivos.Add(_archivo);
        //            //    _dbContext.SaveChanges();
        //            //}
        //        }
        //        catch (Exception ex)
        //        {
        //            // Aquí el código para manejar la Excepción.
        //        }
        //    }

        //    // Redirigimos a la Acción 'Index' para mostrar
        //    // Los archivos subidos al Servidor.
        //    return RedirectToAction("Index");
        //}

        //[HttpGet]
        //public ActionResult DescargarArchivo(Guid id)
        //{
        //Archivo _archivo;
        //FileContentResult _fileContent;

        //using (_dbContext = new AppDbContext())
        //{
        //    _archivo = _dbContext.Archivos.FirstOrDefault(x => x.Id == id);
        //}

        //if (_archivo == null)
        //{
        //    return HttpNotFound();
        //}
        //else
        //{
        //    try
        //    {
        //        // Descargamos el archivo del Servidor.
        //        _fileContent = new FileContentResult(_archivo.DescargarArchivo(),
        //                                             "application/octet-stream");
        //        _fileContent.FileDownloadName = _archivo.Nombre + "." + _archivo.Extension;



        //        return _fileContent;
        //    }
        //    catch (Exception ex)
        //    {
        //        return HttpNotFound();
        //    }
        //}
        //}

        //[HttpGet]
        //public ActionResult EliminarArchivo(Guid id)
        //{
        //    Archivo _archivo = new Archivo();           

        //    if (_archivo != null)
        //    {

        //       // Eliminamos el archivo del Servidor.
        //        _archivo.EliminarArchivo();

        //        // Redirigimos a la Acción 'Index' para mostrar
        //        // Los archivos subidos al Servidor.
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        return HttpNotFound();
        //    }
        //}


    }
}