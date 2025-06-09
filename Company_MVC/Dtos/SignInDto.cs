using System.ComponentModel.DataAnnotations;

namespace Company_MVC.Dtos
{
    public class SignInDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        
        public bool RememberMe { get; set; }
    }
}
