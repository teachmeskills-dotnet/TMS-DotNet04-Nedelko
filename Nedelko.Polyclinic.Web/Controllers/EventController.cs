using Nedelko.Polyclinic.Contexts;
using Nedelko.Polyclinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Nedelko.Polyclinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarEventsController : ControllerBase
    {
        private readonly PolyclinicContext _polyclinicContext;

        public CalendarEventsController(PolyclinicContext polyclinicContext)
        {
            _polyclinicContext = polyclinicContext ?? throw new ArgumentNullException(nameof(polyclinicContext));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var eventQuery =
                _polyclinicContext.Events
                .AsNoTracking()
                .Where(e => !((e.End <= start) || (e.Start >= end)));

            if (User.IsInRole("Patient"))
            {
                eventQuery = eventQuery.Where(e => e.PatientId == Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }

            if (User.IsInRole("Doctor"))
            {
                eventQuery = eventQuery.Where(e => e.DoctorId == Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            }

            return await eventQuery.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetCalendarEvent(int id)
        {
            var calendarEvent = await _polyclinicContext.Events.FindAsync(id);
            if (calendarEvent is null)
            {
                return NotFound();
            }

            return calendarEvent;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalendarEvent(int id, Event calendarEvent)
        {
            if (id != calendarEvent.Id)
            {
                return BadRequest();
            }
            else
            {
                var movedEvent = await _polyclinicContext.Events.FindAsync(id);
                movedEvent.Start = calendarEvent.Start;
                movedEvent.End = calendarEvent.End;
                _polyclinicContext.Entry(movedEvent).State = EntityState.Modified;
            }

            try
            {
                await _polyclinicContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _polyclinicContext.Events.AnyAsync(e => e.Id == id))
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Event>> PostCalendarEvent(Event calendarEvent)
        {
            if (User.IsInRole("Patient"))
            {
                calendarEvent.PatientId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }

            if (User.IsInRole("Doctor"))
            {
                calendarEvent.DoctorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }

            _polyclinicContext.Events.Add(calendarEvent);
            await _polyclinicContext.SaveChangesAsync();

            return CreatedAtAction("GetCalendarEvent", new { id = calendarEvent.Id }, calendarEvent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalendarEvent(int id)
        {
            var calendarEvent = await _polyclinicContext.Events.FindAsync(id);
            if (calendarEvent is null)
            {
                return NotFound();
            }

            _polyclinicContext.Events.Remove(calendarEvent);
            await _polyclinicContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
