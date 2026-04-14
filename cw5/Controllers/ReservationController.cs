using Microsoft.AspNetCore.Mvc;
using cw5.Models;
using cw5.Data;
using cw5.DTOs;

namespace cw5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] DateTime? date,
        [FromQuery] Status? status,
        [FromQuery] int? roomId
    )
    {
        var query = InMemoryData.Reservations.AsEnumerable();

        if (date.HasValue)
        {
            query = query.Where(r => r.Date.Date == date.Value.Date);
        }

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        if (roomId.HasValue)
        {
            query = query.Where(r => r.RoomId == roomId.Value);
        }

        var result = query.Select(r => new ReservationDto
        {
            Id = r.Id,
            RoomId = r.RoomId,
            OrganizerName = r.OrganizerName,
            Topic = r.Topic,
            Date = r.Date,
            StartTime = r.StartTime,
            EndTime = r.EndTime,
            Status = r.Status,

        });

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var reservation = InMemoryData.Reservations.FirstOrDefault(r => r.Id == id);

        if (reservation is null)
        {
            return NotFound($"Rezerwacja o id {id} nie istnieje.");
        }

        var result = new ReservationDto
        {
            Id = reservation.Id,
            RoomId = reservation.RoomId,
            OrganizerName = reservation.OrganizerName,
            Topic = reservation.Topic,
            Date = reservation.Date,
            StartTime = reservation.StartTime,
            EndTime = reservation.EndTime,
            Status = reservation.Status
        };

        return Ok(result);
    }

    [HttpPost]
    public IActionResult Add([FromBody] CreateReservationDto reservationDto)
    {
        var room = InMemoryData.Rooms.FirstOrDefault(r => r.Id == reservationDto.RoomId);

        if (room is null)
        {
            return NotFound($"Sala o id {reservationDto.RoomId} nie istnieje.");
        }

        if (!room.IsActive)
        {
            return Conflict("Nie można dodać rezerwacji do nieaktywnej sali.");
        }

        var reservationDate = reservationDto.Date!.Value.Date;
        var startTime = reservationDate.Add(reservationDto.StartTime!.Value.TimeOfDay);
        var endTime = reservationDate.Add(reservationDto.EndTime!.Value.TimeOfDay);

        var hasConflict = InMemoryData.Reservations.Any(r =>
            r.RoomId == reservationDto.RoomId &&
            r.Date.Date == reservationDate &&
            r.Status != Status.CANCELLED &&
            startTime < r.EndTime &&
            endTime > r.StartTime);

        if (hasConflict)
        {
            return Conflict("Rezerwacja koliduje czasowo z inną rezerwacją tej samej sali.");
        }

        var reservation = new Reservation
        {
            Id = InMemoryData.GetNextReservationId(),
            RoomId = reservationDto.RoomId,
            OrganizerName = reservationDto.OrganizerName,
            Topic = reservationDto.Topic,
            Date = reservationDate,
            StartTime = startTime,
            EndTime = endTime,
            Status = reservationDto.Status!.Value
        };

        InMemoryData.Reservations.Add(reservation);

        var result = new ReservationDto
        {
            Id = reservation.Id,
            RoomId = reservation.RoomId,
            OrganizerName = reservation.OrganizerName,
            Topic = reservation.Topic,
            Date = reservation.Date,
            StartTime = reservation.StartTime,
            EndTime = reservation.EndTime,
            Status = reservation.Status
        };

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, result);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateReservationDto reservationDto)
    {
        var reservation = InMemoryData.Reservations.FirstOrDefault(r => r.Id == id);

        if (reservation is null)
        {
            return NotFound($"Rezerwacja o id {id} nie istnieje.");
        }

        var room = InMemoryData.Rooms.FirstOrDefault(r => r.Id == reservationDto.RoomId);

        if (room is null)
        {
            return NotFound($"Sala o id {reservationDto.RoomId} nie istnieje.");
        }

        if (!room.IsActive)
        {
            return Conflict("Nie można przypisać rezerwacji do nieaktywnej sali.");
        }

        var reservationDate = reservationDto.Date!.Value.Date;
        var startTime = reservationDate.Add(reservationDto.StartTime!.Value.TimeOfDay);
        var endTime = reservationDate.Add(reservationDto.EndTime!.Value.TimeOfDay);

        var hasConflict = InMemoryData.Reservations.Any(r =>
            r.Id != id &&
            r.RoomId == reservationDto.RoomId &&
            r.Date.Date == reservationDate &&
            r.Status != Status.CANCELLED &&
            startTime < r.EndTime &&
            endTime > r.StartTime);

        if (hasConflict)
        {
            return Conflict("Zaktualizowana rezerwacja koliduje z inną rezerwacją tej samej sali.");
        }

        reservation.RoomId = reservationDto.RoomId;
        reservation.OrganizerName = reservationDto.OrganizerName;
        reservation.Topic = reservationDto.Topic;
        reservation.Date = reservationDate;
        reservation.StartTime = startTime;
        reservation.EndTime = endTime;
        reservation.Status = reservationDto.Status!.Value;

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Remove([FromRoute] int id)
    {
        var reservation = InMemoryData.Reservations.FirstOrDefault(r => r.Id == id);

        if (reservation is null)
        {
            return NotFound($"Rezerwacja o id {id} nie istnieje.");
        }

        InMemoryData.Reservations.Remove(reservation);

        return NoContent();
    }
}