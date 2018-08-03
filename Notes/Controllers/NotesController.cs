using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.Models;

namespace Notes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NotesContext _context;

        public NotesController(NotesContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        //[HttpGet]
        //public IEnumerable<Note> GetNote()
        //{

        //    return _context.Note.Include(n=>n.checklist).Include(n=>n.label); //join other tables
        //}

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var note = await _context.Note.Include(n => n.checklist).Include(n => n.label).SingleOrDefaultAsync(c => c.ID == id);


            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote([FromRoute] int id, [FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != note.ID)
            {
                return BadRequest();
            }

            _context.Note.Update(note);
            await _context.SaveChangesAsync();

            try
            {


                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(note);
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<IActionResult> PostNote([FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Note.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { id = note.ID }, note);
        }
        //label
        [HttpGet]
        public async Task<IActionResult> GetNotes([FromQuery] string title, [FromQuery] string label, [FromQuery] bool? pinned)
        {
            var result = await _context.Note.Include(n => n.checklist).Include(n => n.label)
                .Where(x => ((title == null || x.title == title) && (label == null || x.label.Exists(y => y.value == label)) && (pinned == null || x.IsPinned == pinned))).ToListAsync();
            return Ok(result);
        }
        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Note.Include(n => n.checklist).Include(n => n.label).SingleOrDefaultAsync(c => c.ID == id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Note.Remove(note);
            await _context.SaveChangesAsync();

            return Ok(note);
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.ID == id);
        }
        [HttpDelete("delete/{title}")]
        public async Task<IActionResult> DeleteNote([FromRoute] string title)
        {

            var deleteNotes = _context.Note.Include(n => n.checklist).Include(n => n.label).Where(u => u.title == title).ToList();
            foreach (var note in deleteNotes)
            {
                _context.Note.Remove(note);
            }
            await _context.SaveChangesAsync();

            return Ok();
        }

        
    }
}