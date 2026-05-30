using Microsoft.ML.Data;

namespace ApiTareas.Models
{
    public class ComentarioData
    {
        [LoadColumn(0)]
        public string Comentario { get; set; } = string.Empty;

        [LoadColumn(1), ColumnName("Label")]
        public bool EsPositivo { get; set; }
    }

    public class ComentarioPrediction : ComentarioData
    {
        [ColumnName("PredictedLabel")]
        public bool Prediccion { get; set; }

        public float Probability { get; set; }
        public float Score { get; set; }
    }

    public class SentimientoRequest
    {
        public string Comentario { get; set; } = string.Empty;
    }
}
