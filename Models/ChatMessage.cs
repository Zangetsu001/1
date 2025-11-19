using EduMaster.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
[Table("ChatMessages")]
public class ChatMessage
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int RoomId { get; set; }

    public int? SenderId { get; set; }

    [Required]
    public string MessageText { get; set; } = null!;

    [MaxLength(20)]
    public string MessageType { get; set; } = "Text";

    [MaxLength(255)]
    public string? FileUrl { get; set; }

    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public bool IsEdited { get; set; } = false;

    public DateTime? EditedAt { get; set; }

    // Navigation properties
    [ForeignKey("RoomId")]
    public virtual ChatRoom Room { get; set; } = null!;

    [ForeignKey("SenderId")]
    public virtual User? Sender { get; set; }
}