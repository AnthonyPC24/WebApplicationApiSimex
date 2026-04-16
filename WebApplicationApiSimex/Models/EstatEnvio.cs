namespace WebApplicationApiSimex.Models;

public partial class EstatEnvio
{
    public int Id { get; set; }
    public string Nom { get; set; } = null!;
    public virtual ICollection<Oferte> Ofertes { get; set; } = new List<Oferte>();
}