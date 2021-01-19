using System.ComponentModel.DataAnnotations;

namespace BAL.Modelos.Configuracion
{
    public class PerfilXModuloModel
    {
        public int Id { get; set; }

        [Display(Name = "Módulo")]
        public string IdModulo { get; set; }

        [Display(Name = "Perfil")]
        public string IdPerfil { get; set; }

        public int Estado { get; set; }
    }
}