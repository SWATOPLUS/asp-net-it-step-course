using AspNetAngular.Data;
using AspNetAngular.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetAngular.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly AppDbContext _appDb;

        public NoteController(AppDbContext appDb)
        {
            _appDb = appDb;
        }

        [HttpGet]
        public Note[] GetAll()
        {
            return _appDb.Notes
                .AsNoTracking()
                .Where(x => x.ApplicationUserId == GetUserId())
                .ToArray();
        }

        [HttpPost]
        public Note Add([FromBody] Note note)
        {
            note.ApplicationUserId = GetUserId();
            note.NoteId = Guid.NewGuid();

            var entity = _appDb.Notes.Add(note);
            _appDb.SaveChanges();

            return entity.Entity;
        }

        [HttpPut]
        public IActionResult Update([FromBody] Note note)
        {
            var dbNote = _appDb.Notes
                .Where(x => x.ApplicationUserId == GetUserId())
                .Where(x => x.NoteId == note.NoteId)
                .FirstOrDefault();

            if (dbNote == null)
            {
                return BadRequest();
            }

            dbNote.Title = note.Title;
            dbNote.Text = note.Text;
            _appDb.SaveChanges();

            return Ok(dbNote);
        }

        [HttpDelete]
        public IActionResult Remove([FromQuery] Guid noteId)
        {
            var note = _appDb.Notes
                .Where(x => x.ApplicationUserId == GetUserId())
                .Where(x => x.NoteId == noteId)
                .FirstOrDefault();

            if (note == null)
            {
                return BadRequest();
            }

            _appDb.Notes.Remove(note);
            _appDb.SaveChanges();

            return Ok(note);
        }

        private Guid GetUserId()
        {
            var guidString = User.Claims
                .Where(x => x.Type == "ApplicationUserId")
                .Select(x => x.Value)
                .Single();

            return Guid.Parse(guidString);
        }
    }
}
