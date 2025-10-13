using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;
using Sofia.Web.Models;

namespace Sofia.Web.Controllers;

[Route("notes")]
public class NotesController : Controller
{
    private readonly SofiaDbContext _context;

    public NotesController(SofiaDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var notes = await _context.Notes
            .OrderByDescending(n => n.IsPinned)
            .ThenByDescending(n => n.CreatedAt)
            .ToListAsync();
        
        return View(notes);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new Note());
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(Note note)
    {
        if (ModelState.IsValid)
        {
            note.CreatedAt = DateTime.Now;
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(note);
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note == null) return NotFound();
        return View(note);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(int id, Note note)
    {
        if (id != note.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(note);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(note.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(note);
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note != null)
        {
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("toggle-pin/{id}")]
    public async Task<IActionResult> TogglePin(int id)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note != null)
        {
            note.IsPinned = !note.IsPinned;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool NoteExists(int id)
    {
        return _context.Notes.Any(e => e.Id == id);
    }
}


