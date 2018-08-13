using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Notes.Models;

namespace Notes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   public class NotesController : ControllerBase
    {
       private readonly NotesContext _context;
       DataAccess objda;
      public NotesController(DataAccess da)
        {
            objda = da;
        }

      //  private DbContextOptions<NotesContext> options;

        //public NotesController(NotesContext context)
        //{
        //    _context = context;
        //}
        //Get All notes
        [HttpGet]
        public IEnumerable<Note> Get()
        {
            return objda.GetNotes();
        }
        //Get by Id
        public IActionResult Get(string id)
        {
            var note = objda.GetNote(new ObjectId(id));
            if (note == null)
            {
                return NotFound();
            }
            return new ObjectResult(note);
        }

        //Post
        [HttpPost]
        public IActionResult Post([FromBody]Note p)
        {
            objda.Create(p);
            return new OkObjectResult(p);
        }

        //Update
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]Note p)
        {
            var recId = new ObjectId(id);
            var note = objda.GetNote(recId);
            if (note == null)
            {
                return NotFound();
            }

            objda.Update(recId, p);
            return new OkResult();
        }
        //Delete
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var note = objda.GetNote(new ObjectId(id));
            if (note == null)
            {
                return NotFound();
            }

            objda.Remove(note.ID);
            return new OkResult();
        }

    //    GET: api/Notes/5
    //[HttpGet("{id}")]
    //    public async Task<IActionResult> GetNoteById([FromRoute] int id)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }
    //        var note = await _context.Note.Include(n => n.checklist).Include(n => n.label).SingleOrDefaultAsync(c => c.ID == id);


    //        if (note == null)
    //        {
    //            return NotFound();
    //        }

    //        return Ok(note);
    //    }

        //// PUT: api/Notes/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutNote([FromRoute] int id, [FromBody] Note note)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != note.ID)
        //    {
        //        return NotFound();
        //    }

        //    _context.Note.Update(note);

        //    await _context.SaveChangesAsync();

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!NoteExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Ok(note);
        //}

        //// POST: api/Notes
        //[HttpPost]
        //public async Task<IActionResult> PostNote([FromBody] Note note)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.Note.Add(note);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetNoteById", new { id = note.ID }, note);
        //}
        ////label
        //[HttpGet]
        //public async Task<IActionResult> GetNotes([FromQuery] string title, [FromQuery] string label, [FromQuery] bool? pinned)
        //{
        //    var result = await _context.Note.Include(n => n.checklist).Include(n => n.label)
        //        .Where(x => ((title == null || x.title == title) && (label == null || x.label.Exists(y => y.value == label)) && (pinned == null || x.IsPinned == pinned))).ToListAsync();
        //    if (result.Count == 0)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(result);

        //}
        //// DELETE: api/Notes/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteNote([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var note = await _context.Note.Include(n => n.checklist).Include(n => n.label).SingleOrDefaultAsync(c => c.ID == id);
        //    if (note == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Note.Remove(note);
        //    await _context.SaveChangesAsync();

        //    return Ok(note);
        //}

        //private bool NoteExists(int id)
        //{
        //    return _context.Note.Any(e => e.ID == id);
        //}
        //[HttpDelete("delete/{title}")]
        //public async Task<IActionResult> DeleteNote([FromRoute] string title)
        //{

        //    var deleteNotes = _context.Note.Include(n => n.checklist).Include(n => n.label).Where(u => u.title == title).ToList();
        //    foreach (var note in deleteNotes)
        //    {
        //        _context.Note.Remove(note);
        //    }
        //    await _context.SaveChangesAsync();

        //    return Ok();
        //}


    }
}