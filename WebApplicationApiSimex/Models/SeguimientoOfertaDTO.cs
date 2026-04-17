namespace WebApplicationApiSimex.Models
{
    public class SeguimientoOfertaDTO
    {

        public int OfertaId { get; set; }
        public string IncotermNombre { get; set; }
        // Aquí incluimos la lista de pasos ya cruzada con su estado actual
        public List<PasoSeguimientoDTO> Pasos { get; set; }

    }

    public class PasoSeguimientoDTO
    {
        public int TrackingStepId { get; set; }
        public string NombrePaso { get; set; }
        public int Orden { get; set; }
        public int? EstadoActualId { get; set; } // 1: Pendiente, 2: En curso, 3: Finalizado
    }
}
