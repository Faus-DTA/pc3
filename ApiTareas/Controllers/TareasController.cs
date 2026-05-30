using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTareas.Data;
using ApiTareas.Models;

namespace ApiTareas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TareasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/tareas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas()
        {
            return await _context.Tareas.ToListAsync();
        }

        // GET: api/tareas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarea>> GetTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);

            if (tarea == null)
            {
                return NotFound(new { message = $"No se encontró la tarea con el ID {id}." });
            }

            return tarea;
        }

        // POST: api/tareas
        [HttpPost]
        public async Task<ActionResult<Tarea>> PostTarea(Tarea tarea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure FechaCreacion is set to current if not provided correctly, though default sets it.
            tarea.FechaCreacion = DateTime.Now;

            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTarea), new { id = tarea.Id }, tarea);
        }

        // PUT: api/tareas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarea(int id, Tarea tarea)
        {
            if (id != tarea.Id)
            {
                return BadRequest(new { message = "El ID de la ruta no coincide con el ID de la tarea." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tareaExistente = await _context.Tareas.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
            if (tareaExistente == null)
            {
                return NotFound(new { message = $"No se encontró la tarea con el ID {id} para actualizar." });
            }

            // Preserve FechaCreacion
            tarea.FechaCreacion = tareaExistente.FechaCreacion;

            _context.Entry(tarea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TareaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(tarea);
        }

        // DELETE: api/tareas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
            {
                return NotFound(new { message = $"No se encontró la tarea con el ID {id} para eliminar." });
            }

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Tarea eliminada exitosamente." });
        }

        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(e => e.Id == id);
        }
    }
}
