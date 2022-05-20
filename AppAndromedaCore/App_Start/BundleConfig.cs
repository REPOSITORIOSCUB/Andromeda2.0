using System.Web;
using System.Web.Optimization;

namespace AppAndromedaCore
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));



            bundles.Add(new StyleBundle("~/bundles/AdminLT").Include(
                     "~/Content/AdminLT/plugins/fontawesome-free/css/all.min.css",
                     "~/Content/AdminLT/Site.css",
                     "~/Content/AdminLT/bootstrap.css",
                     "~/Content/AdminLT/plugins/sweetalert2/sweetalert2.min.css",
                     "~/Content/AdminLT/plugins/sweetalert2-theme-bootstrap-4/bootstrap-4.min.css",
                     "~/Content/AdminLT/plugins/datatables-bs4/css/dataTables.bootstrap4.min.css",
                     "~/Content/AdminLT/plugins/datatables-responsive/css/responsive.bootstrap4.min.css",
                     "~/Content/AdminLT/dist/css/adminlte.css",
                     "~/Content/AdminLT/plugins/daterangepicker/daterangepicker.css",
                     "~/Content/AdminLT/plugins/jquery-ui/themes/cupertino/jquery-ui.min.css",
                     "~/Content/AdminLT/plugins/jquery-ui/themes/cupertino/theme.css",
                     "~/Content/AdminLT/plugins/bootstrap-toggle/bootstrap-toggle.min.css",
                     "~/Content/AdminLT/plugins/icheck-bootstrap/icheck-bootstrap.min.css",
                     "~/Content/AdminLT/plugins/plugins/bootstrap4-duallistbox/bootstrap-duallistbox.css",
                     "~/Content/AdminLT/plugins/select2/css/select2.min.css",
                     "~/Content/AdminLT/plugins/select2-bootstrap4-theme/select2-bootstrap4.min.css",
                     "~/Content/AdminLT/dist/css/PersonalizadosSitio.css",
                     "~/Content/Popup/estilos.css"
                     ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css",
                      //"~/Content/site.css"
                      "~/Content/jquery.steps.css"
                     ));


            bundles.Add(new ScriptBundle("~/bundles/AdminLT/plugins").Include(
                    "~/Content/AdminLT/plugins/jquery/jquery.min.js",
                    "~/Content/AdminLT/plugins/bootstrap/js/bootstrap.bundle.min.js",
                    "~/Content/AdminLT/plugins/select2/js/select2.full.min.js",
                    "~/Content/AdminLT/plugins/sweetalert2/sweetalert2.js",
                    "~/Content/AdminLT/plugins/bs-custom-file-input/bs-custom-file-input.min.js",
                    "~/Content/AdminLT/plugins/datatables/jquery.dataTables.js",
                    "~/Content/AdminLT/plugins/datatables-bs4/js/dataTables.bootstrap4.min.js",
                    "~/Content/AdminLT/plugins/datatables-responsive/js/dataTables.responsive.min.js",
                    "~/Content/AdminLT/plugins/datatables-responsive/js/responsive.bootstrap4.min.js",
                    "~/Content/AdminLT/plugins/jquery-ui/jquery-ui.js",
                    "~/Content/AdminLT/plugins/daterangepicker/daterangepicker.js",
                    "~/Content/AdminLT/plugins/bootstrap-toggle/bootstrap-toggle.min.js",
                    "~/Content/AdminLT/plugins/bootstrap4-duallistbox/jquery.bootstrap-duallistbox.js",
                    "~/Content/AdminLT/plugins/bs-custom-file-input/bs-custom-file-input.min.js",
                    "~/Content/Popup/popup.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/AdminLT/js").Include(
                   "~/Content/AdminLT/dist/js/adminlte.js",
                   "~/Scripts/jquery.steps.js"
                   ));


        }
    }
}
