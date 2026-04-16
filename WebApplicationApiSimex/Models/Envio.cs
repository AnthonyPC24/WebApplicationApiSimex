namespace WebApplicationApiSimex.Models;

public partial class Envio
{
    public int Id { get; set; }
    public string Origen { get; set; } = null!;
    public string Destino { get; set; } = null!;
    public string EstadoEnvio { get; set; } = null!;
    public string OfertaId { get; set; } = null!;
    public string ContenidoEnvio { get; set; } = null!;
    public string MetodoTransporte { get; set; } = null!;
    public string TipoDivisa { get; set; } = null!;
    public DateOnly FechaPedido { get; set; }
    public string AgenteComercial { get; set; } = null!;
    public string Ruta { get; set; } = null!;
    public decimal PesoKg { get; set; }
    public string Incoterm { get; set; } = null!;
    public string Urgencia { get; set; } = null!;
    public string Compania { get; set; } = null!;
    public int ClienteId { get; set; }  // ← era agente_id
}