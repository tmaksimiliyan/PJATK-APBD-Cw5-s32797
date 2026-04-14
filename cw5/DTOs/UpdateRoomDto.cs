using System.ComponentModel.DataAnnotations;

namespace cw5.DTOs;

public class UpdateRoomDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(10)]
    public string BuildingCode { get; set; } = string.Empty;
    
    [Range(0, 100)]
    public int Floor { get; set; }
    
    [Range(0, int.MaxValue)]
    public int Capacity { get; set; }
    
    public bool HasProjector { get; set; }
    
    public bool IsActive { get; set; }
}