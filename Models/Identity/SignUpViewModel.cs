using System.ComponentModel.DataAnnotations;

namespace hki.web.Models.Identity
{
    public class SignUpViewModel
    {
        
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.Password, ErrorMessage = "El password no es valido")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Los passwords no concuerdan, intente de nuevo")]
        [Display(Name = "Confirmar Password")]
        public string RePassword { get; set; }

        public Roles Rol { get; set; }
    }

    public enum Roles
    {
        Administrador,
        Produccion,
        Almacen,
        Calidad,
        Programacion
    }
}