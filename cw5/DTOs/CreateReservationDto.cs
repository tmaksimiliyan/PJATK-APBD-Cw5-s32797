using System.ComponentModel.DataAnnotations;
using cw5.Models;

namespace cw5.DTOs;

public class CreateReservationDto : IValidatableObject
{
    [Range(1, int.MaxValue)]
    public int RoomId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string OrganizerName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Topic { get; set; }   = string.Empty;
    
    [Required]
    public DateTime? Date {get; set;}
    
    [Required]
    public DateTime? StartTime { get; set; }
    
    [Required]
    public DateTime? EndTime { get; set; }
    
    [Required]
    public Status? Status { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartTime.HasValue && EndTime.HasValue && EndTime <= StartTime)
        {
            yield return new ValidationResult(
                "EndTime must be after StartTime",
                new[] { nameof(StartTime),  nameof(EndTime) });
        }
    }
}