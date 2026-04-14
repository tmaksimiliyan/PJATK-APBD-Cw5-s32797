namespace cw5.Models;

public class Reservation
{
    public int Id { get; set; }

    public int RoomId { get; set; }
    
    public string OrganizerName { get; set; } = string.Empty;
    
    public string Topic { get; set; }   = string.Empty;
    
    public DateTime Date {get; set;}
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public Status Status { get; set; }
}