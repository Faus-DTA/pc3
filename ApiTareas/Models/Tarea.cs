using System;
using System.ComponentModel.DataAnnotations;

namespace ApiTareas.Models
{
    public enum EstadoTarea
    {
        Pendiente,
        EnProceso,
        Completada
    }

    public enum PrioridadTarea
    {
        Baja,
        Media,
        Alta
    }

    public class Tarea
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        public string Titulo { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public EstadoTarea Estado { get; set; }

        [Required(ErrorMessage = "La prioridad es obligatoria.")]
        public PrioridadTarea Prioridad { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
        [FutureDate(ErrorMessage = "La fecha de vencimiento no puede ser menor a la fecha actual.")]
        public DateTime FechaVencimiento { get; set; }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime dateTime)
            {
                // Permitimos un pequeño margen por si se envía justo en el momento actual, pero la regla general es > DateTime.Now
                return dateTime.Date >= DateTime.Now.Date;
            }
            return false;
        }
    }
}
