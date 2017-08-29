using System.ComponentModel.DataAnnotations;

namespace hki.web.Models.Identity
{
    public class LoginViewModel
    {
        [Required]
        public string Usuario { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}