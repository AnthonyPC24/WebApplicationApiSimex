using System;
using System.Collections.Generic;

namespace WebApplicationApiSimex.Models;

public partial class TipusTransport
{
    public int Id { get; set; }

    public string Tipus { get; set; } = null!;

    public virtual ICollection<Oferte> Ofertes { get; set; } = new List<Oferte>();
}
