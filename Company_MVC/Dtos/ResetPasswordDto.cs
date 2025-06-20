using System.ComponentModel.DataAnnotations;

namespace Company_MVC.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "NewPassword is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
