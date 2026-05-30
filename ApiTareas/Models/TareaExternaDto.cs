using System.Text.Json.Serialization;

namespace ApiTareas.Models
{
    // Clase para mapear la respuesta de la API externa
    public class JsonPlaceholderTodo
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("completed")]
        public bool Completed { get; set; }
    }

    // DTO propio solicitado en los requerimientos
    public class TareaExternaDto
    {
        public int externalId { get; set; }
        public string titulo { get; set; } = string.Empty;
        public bool completado { get; set; }
    }
}
