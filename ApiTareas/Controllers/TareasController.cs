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
        public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas([FromQuery] string? estado, [FromQuery] string? prioridad, [FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio > fechaFin)
            {
                return BadRequest(new { message = "La fecha de inicio no puede ser mayor que la fecha de fin." });
            }

            var query = _context.Tareas.AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                if (!Enum.TryParse<EstadoTarea>(estado, true, out var estadoParsed))
                {
                    return BadRequest(new { message = "El estado proporcionado no es válido. Valores permitidos: Pendiente, EnProceso, Completada." });
                }
                query = query.Where(t => t.Estado == estadoParsed);
            }

            if (!string.IsNullOrEmpty(prioridad))
            {
                if (!Enum.TryParse<PrioridadTarea>(prioridad, true, out var prioridadParsed))
                {
                    return BadRequest(new { message = "La prioridad proporcionada no es válida. Valores permitidos: Baja, Media, Alta." });
                }
                query = query.Where(t => t.Prioridad == prioridadParsed);
            }

            if (fechaInicio.HasValue)
            {
                query = query.Where(t => t.FechaVencimiento >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(t => t.FechaVencimiento <= fechaFin.Value);
            }

            return await query.ToListAsync();
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
