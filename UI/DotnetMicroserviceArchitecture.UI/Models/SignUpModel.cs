using System.ComponentModel.DataAnnotations;

namespace DotnetMicroserviceArchitecture.UI.Models
{
    public class SignUpModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool IsRemember { get; set; }
    }
}
