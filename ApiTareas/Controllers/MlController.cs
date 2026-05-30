using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using ApiTareas.Models;

namespace ApiTareas.Controllers
{
    [Route("api/ml")]
    [ApiController]
    public class MlController : ControllerBase
    {
        private readonly MLContext _mlContext;
        private readonly PredictionEngine<ComentarioData, ComentarioPrediction> _predictionEngine;

        public MlController()
        {
            _mlContext = new MLContext();

            // 1. Dataset simple en memoria
            var datosEntrenamiento = new List<ComentarioData>
            {
                // Positivos (true)
                new ComentarioData { Comentario = "Excelente trabajo", EsPositivo = true },
                new ComentarioData { Comentario = "Completado a tiempo", EsPositivo = true },
                new ComentarioData { Comentario = "El sistema funciona bien", EsPositivo = true },
                new ComentarioData { Comentario = "Muy buena tarea", EsPositivo = true },
                new ComentarioData { Comentario = "Perfecto, sin errores", EsPositivo = true },
                
                // Negativos (false)
                new ComentarioData { Comentario = "Hay errores", EsPositivo = false },
                new ComentarioData { Comentario = "No funciona", EsPositivo = false },
                new ComentarioData { Comentario = "Mal resultado", EsPositivo = false },
                new ComentarioData { Comentario = "Retrasado", EsPositivo = false },
                new ComentarioData { Comentario = "Pésimo servicio", EsPositivo = false }
            };

            // 2. Cargar datos a IDataView
            var dataView = _mlContext.Data.LoadFromEnumerable(datosEntrenamiento);

            // 3. Crear el pipeline de entrenamiento
            var pipeline = _mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(ComentarioData.Comentario))
                .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

            // 4. Entrenar el modelo
            var model = pipeline.Fit(dataView);

            // 5. Crear el motor de predicción
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<ComentarioData, ComentarioPrediction>(model);
        }

        [HttpPost("sentimiento")]
        public IActionResult AnalizarSentimiento([FromBody] SentimientoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Comentario))
            {
                return BadRequest(new { message = "El comentario no puede estar vacío." });
            }

            // Realizar la predicción
            var input = new ComentarioData { Comentario = request.Comentario };
            var prediction = _predictionEngine.Predict(input);

            // Armar la respuesta
            var resultado = new
            {
                comentario = request.Comentario,
                sentimiento = prediction.Prediccion ? "Positivo" : "Negativo"
            };

            return Ok(resultado);
        }
    }
}
