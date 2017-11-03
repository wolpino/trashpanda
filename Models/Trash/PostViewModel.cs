using System.ComponentModel.DataAnnotations;
namespace trashpanda.Models

{
    public class PostViewModel : BaseEntity
    {
        [Required]
        public string idea { get; set; }

    }
}
    