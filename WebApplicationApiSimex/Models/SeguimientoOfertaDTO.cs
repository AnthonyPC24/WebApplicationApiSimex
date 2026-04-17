using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApplicationApiSimex.Models
{
    /// <summary>
    /// Objeto principal que contiene la información de la oferta y su lista de hitos
    /// </summary>
    public class SeguimientoOfertaDTO
    {
        [JsonPropertyName("ofertaId")]
        public int OfertaId { get; set; }

        [JsonPropertyName("incotermNombre")]
        public string IncotermNombre { get; set; }

        [JsonPropertyName("estatEnvioGeneralId")]
        public int? EstatEnvioGeneralId { get; set; }

        [JsonPropertyName("pasos")]
        public List<PasoSeguimientoDTO> Pasos { get; set; } = new List<PasoSeguimientoDTO>();
    }

    /// <summary>
    /// Representa cada uno de los pasos o hitos del Incoterm
    /// </summary>
    public class PasoSeguimientoDTO
    {
        [JsonPropertyName("trackingStepId")]
        public int TrackingStepId { get; set; }

        [JsonPropertyName("nombrePaso")]
        public string NombrePaso { get; set; }

        [JsonPropertyName("orden")]
        public int Orden { get; set; }

        /// <summary>
        /// ID del estado actual del hito (1: Preparación, 2: Envío, 3: Finalizado)
        /// </summary>
        [JsonPropertyName("estadoActualId")]
        public int? EstadoActualId { get; set; }
    }
}