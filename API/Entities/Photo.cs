using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public bool IsMail { get; set; }
        public string? PublicId { get; set; }

        //one to many
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; } = null!;
    }
}