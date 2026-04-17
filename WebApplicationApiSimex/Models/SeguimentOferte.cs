using System;
using System.Collections.Generic;

namespace WebApplicationApiSimex.Models;

public partial class SeguimentOferte
{
    public int Id { get; set; }

    public int OfertaId { get; set; }

    public int TrackingStepId { get; set; }

    public int EstatId { get; set; }

    public DateTime? DataActualitzacio { get; set; }

    public virtual EstatsEnvio Estat { get; set; } = null!;

    public virtual Oferte Oferta { get; set; } = null!;

    public virtual TrackingStep TrackingStep { get; set; } = null!;
}
