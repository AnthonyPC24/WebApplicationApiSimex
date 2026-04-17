using System;
using System.Collections.Generic;

namespace WebApplicationApiSimex.Models;

public partial class Usuari
{
    public int Id { get; set; }

    public string Correu { get; set; } = null!;

    public string Contrasenya { get; set; } = null!;

    public string Nom { get; set; } = null!;

    public string Cognoms { get; set; } = null!;

    public string Empresa { get; set; } = null!;

    public int RolId { get; set; }

    public string? Telefon { get; set; }

    public string? Cif { get; set; }

    public bool? Actiu { get; set; }

    public string? DniFoto { get; set; }

    public virtual ICollection<Envio> Envios { get; set; } = new List<Envio>();

    public virtual ICollection<Oferte> Ofertes { get; set; } = new List<Oferte>();

    public virtual Rol Rol { get; set; } = null!;
}
