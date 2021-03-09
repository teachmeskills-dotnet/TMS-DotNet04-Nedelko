using DBTestCreator_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DBTestCreator_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarEventsController : ControllerBase
    {
        private readonly MyContext _myContext;

        public CalendarEventsController(MyContext myContext)
        {
            _myContext = myContext;
        }

        // GET: api/CalendarEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarEvent>>> GetEvents([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            if (User.IsInRole("Patient"))
            {
                return await _myContext.Events
                                .Where(e => !((e.End <= start) || (e.Start >= end)))
                                .Where(e => e.PatientId == Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                                //.Where(e => e.DoctorId == id)
                                .ToListAsync();
            }
            else if (User.IsInRole("Doctor"))
            {
                return await _myContext.Events
                                .Where(e => !((e.End <= start) || (e.Start >= end)))
                                .Where(e => e.DoctorId == Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                                .ToListAsync();
            }
            else
            {
                return await _myContext.Events
                                .Where(e => !((e.End <= start) || (e.Start >= end)))
                                .ToListAsync();
            }
        }

        // GET: api/CalendarEvents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarEvent>> GetCalendarEvent(int id)
        {
            var calendarEvent = await _myContext.Events.FindAsync(id);

            if (calendarEvent == null)
            {
                return NotFound();
            }

            return calendarEvent;
        }

        // PUT: api/CalendarEvents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalendarEvent(int id, CalendarEvent calendarEvent)
        {
            if (id != calendarEvent.Id)
            {
                return BadRequest();
            }
            else
            {
                var movedEvent = await _myContext.Events.FindAsync(id);
                movedEvent.Start = calendarEvent.Start;
                movedEvent.End = calendarEvent.End;
                _myContext.Entry(movedEvent).State = EntityState.Modified;
            }

            try
            {
                await _myContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalendarEventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CalendarEvents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CalendarEvent>> PostCalendarEvent(CalendarEvent calendarEvent)
        {
            if (User.IsInRole("Patient"))
            {
                calendarEvent.PatientId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            else if (User.IsInRole("Doctor"))
            {
                calendarEvent.DoctorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            _myContext.Events.Add(calendarEvent);
            await _myContext.SaveChangesAsync();

            return CreatedAtAction("GetCalendarEvent", new { id = calendarEvent.Id }, calendarEvent);
        }

        // DELETE: api/CalendarEvents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalendarEvent(int id)
        {
            var calendarEvent = await _myContext.Events.FindAsync(id);
            if (calendarEvent == null)
            {
                return NotFound();
            }

            _myContext.Events.Remove(calendarEvent);
            await _myContext.SaveChangesAsync();

            return NoContent();
        }

        private bool CalendarEventExists(int id)
        {
            return _myContext.Events.Any(e => e.Id == id);
        }
    }
}
