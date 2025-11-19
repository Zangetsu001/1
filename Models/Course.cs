using EduMaster.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EduMaster.Models
{
    public class Course
    {
    }
}
[Table("Courses")]
public class Course
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? TeacherId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    // Navigation properties
    [ForeignKey("TeacherId")]
    public virtual User? Teacher { get; set; }
    public virtual ICollection<ChatRoom> ChatRooms { get; set; } = new List<ChatRoom>();
}