using cw5.Models;

namespace cw5.Data;

public class InMemoryData
{
    public static List<Room> Rooms { get; } = new()
    {
        new Room
        {
            Id = 1,
            Name = "Aula A1",
            BuildingCode = "A",
            Floor = 0,
            Capacity = 150,
            HasProjector = true,
            IsActive = true,
        },
        new Room
        {
            Id = 2,
            Name = "Aula B1",
            BuildingCode = "B",
            Floor = 0,
            Capacity = 150,
            HasProjector = true,
            IsActive = true,
        },
        new Room
        {
            Id = 3,
            Name = "Sala A359",
            BuildingCode = "A",
            Floor = 3,
            Capacity = 20,
            HasProjector = true,
            IsActive = true,
        },
        new Room
        {
            Id = 4,
            Name = "Sala A260",
            BuildingCode = "A",
            Floor = 2,
            Capacity = 20,
            HasProjector = true,
            IsActive = true,
        },
        new Room
        {
            Id = 5,
            Name = "Sala B226/A",
            BuildingCode = "B",
            Floor = 2,
            Capacity = 20,
            HasProjector = true,
            IsActive = true,
        }
    };

    public static List<Reservation> Reservations { get; } = new()
    {
        new Reservation()
        {
            Id = 1,
            RoomId = 1,
            OrganizerName = "Pieciukiewicz Tomasz",
            Topic = "UML",
            Date = new DateTime(2026, 04, 17),
            StartTime = new DateTime(2026, 04, 17, 8, 30, 0),
            EndTime = new DateTime(2026, 04, 17, 10, 0, 0),
            Status = Status.CONFIRMED
        },
        new Reservation()
        {
            Id = 2,
            RoomId = 2,
            OrganizerName = "Gago Piotr",
            Topic = "ASP.NET",
            Date = new DateTime(2026, 04, 14),
            StartTime = new DateTime(2026, 04, 14, 15, 30, 0),
            EndTime = new DateTime(2026, 04, 14, 17, 15, 0),
            Status = Status.CONFIRMED
        },
        new Reservation()
        {
            Id = 3,
            RoomId = 3,
            OrganizerName = "Otowski Kacper",
            Topic = "ASP.NET",
            Date = new DateTime(2026, 04, 16),
            StartTime = new DateTime(2026, 04, 16, 12, 15, 0),
            EndTime = new DateTime(2026, 04, 16, 13, 45, 0),
            Status = Status.CONFIRMED
        },
        new Reservation()
        {
            Id = 4,
            RoomId = 4,
            OrganizerName = "Pierzchała Szymon",
            Topic = "Kanały plikowe",
            Date = new DateTime(2026, 04, 16),
            StartTime = new DateTime(2026, 04, 16, 15, 30, 0),
            EndTime = new DateTime(2026, 04, 16, 17, 15, 0),
            Status = Status.CONFIRMED
        },
        new Reservation()
        {
            Id = 5,
            RoomId = 5,
            OrganizerName = "Jastrzębska Ewa",
            Topic = "Słownik",
            Date = new DateTime(2026, 04, 14),
            StartTime = new DateTime(2026, 04, 14, 14, 0, 0),
            EndTime = new DateTime(2026, 04, 14, 15, 30, 0),
            Status = Status.CONFIRMED
        }
    };
    
    public static int GetNextRoomId()
    {
        return Rooms.Count == 0 ? 1 : Rooms.Max(r => r.Id) + 1;
    }
    
    public static int GetNextReservationId()
    {
        return Reservations.Count == 0 ? 1 : Reservations.Max(r => r.Id) + 1;
    }
}