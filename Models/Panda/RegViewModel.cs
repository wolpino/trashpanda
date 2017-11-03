using System.ComponentModel.DataAnnotations;
namespace trashpanda.Models

{
    public class RegViewModel : BaseEntity
    {
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Only letters please!")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Only letters please!")]
        [Display(Name = "Alias")]
        public string Alias { get; set; }
 
        [Required]
        [EmailAddress]
        public string Email { get; set; }
 
        [Required]
        [MinLength(8, ErrorMessage = "Password must be more than 8 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
 
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
