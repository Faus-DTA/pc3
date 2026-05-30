using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiTareas.Models;

namespace ApiTareas.Controllers
{
    [Route("api/tareas-externas")]
    [ApiController]
    public class TareasExternasController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TareasExternasController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExternas()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("JsonPlaceholder");
                var response = await client.GetAsync("todos");

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, new { message = "Error al comunicarse con la API externa." });
                }

                var todos = await response.Content.ReadFromJsonAsync<List<JsonPlaceholderTodo>>();
                
                if (todos == null)
                {
                    return StatusCode(500, new { message = "Error al procesar la respuesta de la API externa." });
                }

                var dtos = todos.Select(t => new TareaExternaDto
                {
                    externalId = t.Id,
                    titulo = t.Title,
                    completado = t.Completed
                }).ToList();

                return Ok(dtos);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, new { message = "La API externa no responde o falló la conexión.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado al consumir la API externa.", detalle = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExternaById(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("JsonPlaceholder");
                var response = await client.GetAsync($"todos/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(new { message = $"No se encontró la tarea externa con el ID {id}." });
                }

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, new { message = "Error al comunicarse con la API externa." });
                }

                var todo = await response.Content.ReadFromJsonAsync<JsonPlaceholderTodo>();

                if (todo == null)
                {
                    return StatusCode(500, new { message = "Error al procesar la respuesta de la API externa." });
                }

                var dto = new TareaExternaDto
                {
                    externalId = todo.Id,
                    titulo = todo.Title,
                    completado = todo.Completed
                };

                return Ok(dto);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, new { message = "La API externa no responde o falló la conexión.", detalle = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado al consumir la API externa.", detalle = ex.Message });
            }
        }
    }
}
