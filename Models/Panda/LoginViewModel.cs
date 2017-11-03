using System.ComponentModel.DataAnnotations;
namespace trashpanda.Models

{
    public class LoginViewModel : BaseEntity
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Username { get; set; }
 
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PWD { get; set; }

    }
}
    