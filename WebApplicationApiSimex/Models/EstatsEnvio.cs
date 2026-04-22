using System;
using System.Collections.Generic;

namespace WebApplicationApiSimex.Models;

public partial class EstatsEnvio
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public virtual ICollection<Oferte> Ofertes { get; set; } = new List<Oferte>();

    public virtual ICollection<SeguimentOferte> SeguimentOfertes { get; set; } = new List<SeguimentOferte>();
}
