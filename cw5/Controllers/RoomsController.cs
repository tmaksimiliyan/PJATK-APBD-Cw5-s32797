using Microsoft.AspNetCore.Mvc;
using cw5.Models;
using cw5.Data;
using cw5.DTOs;

namespace cw5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        var query = InMemoryData.Rooms.AsEnumerable();

        if (minCapacity.HasValue)
        {
            query = query.Where(r => r.Capacity >= minCapacity.Value);
        }

        if (hasProjector.HasValue)
        {
            query = query.Where(r => r.HasProjector == hasProjector.Value);
        }

        if (activeOnly.HasValue && activeOnly.Value)
        {
            query = query.Where(r => r.IsActive);
        }

        var result = query.Select(r => new RoomDto
        {
            Id = r.Id,
            Name = r.Name,
            BuildingCode = r.BuildingCode,
            Floor = r.Floor,
            Capacity = r.Capacity,
            HasProjector = r.HasProjector,
            IsActive = r.IsActive,
        });

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var room = InMemoryData.Rooms.FirstOrDefault(r => r.Id == id);

        if (room is null)
        {
            return NotFound($"Sala o id {id} nie istnieje");
        }

        var result = new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            BuildingCode = room.BuildingCode,
            Floor = room.Floor,
            Capacity =  room.Capacity,
            HasProjector = room.HasProjector,
            IsActive =  room.IsActive
        };
        
        return Ok(result);
    }

    [HttpGet("buildings/{buildingCode}")]
    public IActionResult GetByBuildingCode([FromRoute] string buildingCode)
    {
        var rooms = InMemoryData.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .Select(r => new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                BuildingCode =  r.BuildingCode,
                Floor = r.Floor,
                Capacity = r.Capacity,
                HasProjector =  r.HasProjector,
                IsActive = r.IsActive
            })
            .ToList();
        
        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult Add([FromBody] CreateRoomDto roomDto)
    {
        var room = new Room
        {
            Id = InMemoryData.GetNextRoomId(),
            Name = roomDto.Name,
            BuildingCode = roomDto.BuildingCode,
            Floor = roomDto.Floor,
            Capacity = roomDto.Capacity,
            HasProjector = roomDto.HasProjector,
            IsActive = roomDto.IsActive
        };
        
        InMemoryData.Rooms.Add(room);

        var result = new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            BuildingCode = room.BuildingCode,
            Capacity = room.Capacity,
            HasProjector = room.HasProjector,
            IsActive = room.IsActive
        };
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateRoomDto roomDto)
    {
        var room = InMemoryData.Rooms.FirstOrDefault(r => r.Id == id);

        if (room is null)
        {
            return NotFound($"Sala o id {id} nie istnieje");
        }
        
        room.Name = roomDto.Name;
        room.BuildingCode = roomDto.BuildingCode;
        room.Floor = roomDto.Floor;
        room.Capacity = roomDto.Capacity;
        room.HasProjector = roomDto.HasProjector;
        room.IsActive = roomDto.IsActive;
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var room = InMemoryData.Rooms.FirstOrDefault(r => r.Id == id);

        if (room is null)
        {
            return NotFound($"Sala o id {id} nie istnieje");
        }
        
        var hasRelatedReservations = InMemoryData.Reservations.Any(r => r.RoomId == id);

        if (hasRelatedReservations)
        {
            return Conflict("Nie można usunąć sali, ponieważ ma powiązane rezerwacje");
        }
        
        InMemoryData.Rooms.Remove(room);
        
        return NoContent();
    }
}