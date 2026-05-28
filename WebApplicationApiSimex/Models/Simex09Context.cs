using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationApiSimex.Models;

public partial class Simex09Context : DbContext
{
    public Simex09Context()
    {
    }

    public Simex09Context(DbContextOptions<Simex09Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Aeroport> Aeroports { get; set; }

    public virtual DbSet<Cache> Caches { get; set; }

    public virtual DbSet<CacheLock> CacheLocks { get; set; }

    public virtual DbSet<Ciutat> Ciutats { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Envio> Envios { get; set; }

    public virtual DbSet<EstatsEnvio> EstatsEnvios { get; set; }

    public virtual DbSet<EstatsOferte> EstatsOfertes { get; set; }

    public virtual DbSet<FailedJob> FailedJobs { get; set; }

    public virtual DbSet<Incoterm> Incoterms { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobBatch> JobBatches { get; set; }

    public virtual DbSet<LiniesTransportMaritim> LiniesTransportMaritims { get; set; }

    public virtual DbSet<Migration> Migrations { get; set; }

    public virtual DbSet<Oferte> Ofertes { get; set; }

    public virtual DbSet<Paisso> Paissos { get; set; }

    public virtual DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    public virtual DbSet<PersonalAccessToken> PersonalAccessTokens { get; set; }

    public virtual DbSet<Port> Ports { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<SeguimentOferte> SeguimentOfertes { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<TipusCarrega> TipusCarregas { get; set; }

    public virtual DbSet<TipusContenidor> TipusContenidors { get; set; }

    public virtual DbSet<TipusFlux> TipusFluxes { get; set; }

    public virtual DbSet<TipusIncoterm> TipusIncoterms { get; set; }

    public virtual DbSet<TipusTransport> TipusTransports { get; set; }

    public virtual DbSet<TipusValidacion> TipusValidacions { get; set; }

    public virtual DbSet<TrackingStep> TrackingSteps { get; set; }

    public virtual DbSet<Transportiste> Transportistes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Usuari> Usuaris { get; set; }

    public virtual DbSet<Partida> Partides { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=51.83.192.177;Database=simex09;User id=simex09;Password=Db@Secure_26!xP;Encrypt=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aeroport>(entity =>
        {
            entity.ToTable("aeroports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CiutatId).HasColumnName("ciutat_id");
            entity.Property(e => e.Codi)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("codi");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");

            entity.HasOne(d => d.Ciutat).WithMany(p => p.Aeroports)
                .HasForeignKey(d => d.CiutatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_aeroports_ciutats");
        });

        modelBuilder.Entity<Cache>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("cache_key_primary");

            entity.ToTable("cache");

            entity.HasIndex(e => e.Expiration, "cache_expiration_index");

            entity.Property(e => e.Key)
                .HasMaxLength(255)
                .HasColumnName("key");
            entity.Property(e => e.Expiration).HasColumnName("expiration");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<CacheLock>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("cache_locks_key_primary");

            entity.ToTable("cache_locks");

            entity.HasIndex(e => e.Expiration, "cache_locks_expiration_index");

            entity.Property(e => e.Key)
                .HasMaxLength(255)
                .HasColumnName("key");
            entity.Property(e => e.Expiration).HasColumnName("expiration");
            entity.Property(e => e.Owner)
                .HasMaxLength(255)
                .HasColumnName("owner");
        });

        modelBuilder.Entity<Ciutat>(entity =>
        {
            entity.ToTable("ciutats");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.PaisId).HasColumnName("pais_id");

            entity.HasOne(d => d.Pais).WithMany(p => p.Ciutats)
                .HasForeignKey(d => d.PaisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ciutats_paissos");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("clients");

            entity.Property(e => e.Id).HasColumnName("id");
        });

        modelBuilder.Entity<Envio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__envios__3213E83F69F60150");

            entity.ToTable("envios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cliente)
                .HasMaxLength(100)
                .HasColumnName("cliente");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.Compania)
                .HasMaxLength(150)
                .HasColumnName("compania");
            entity.Property(e => e.ContenidoEnvio)
                .HasMaxLength(255)
                .HasColumnName("contenido_envio");
            entity.Property(e => e.Destino)
                .HasMaxLength(100)
                .HasColumnName("destino");
            entity.Property(e => e.EstadoEnvio)
                .HasMaxLength(50)
                .HasColumnName("estado_envio");
            entity.Property(e => e.FechaPedido).HasColumnName("fecha_pedido");
            entity.Property(e => e.Incoterm)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("incoterm");
            entity.Property(e => e.MetodoTransporte)
                .HasMaxLength(50)
                .HasColumnName("metodo_transporte");
            entity.Property(e => e.OfertaId)
                .HasMaxLength(20)
                .HasColumnName("oferta_id");
            entity.Property(e => e.Origen)
                .HasMaxLength(100)
                .HasColumnName("origen");
            entity.Property(e => e.PesoKg)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("peso_kg");
            entity.Property(e => e.Ruta)
                .HasMaxLength(200)
                .HasColumnName("ruta");
            entity.Property(e => e.TipoDivisa)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("tipo_divisa");
            entity.Property(e => e.Urgencia)
                .HasMaxLength(10)
                .HasColumnName("urgencia");

            entity.HasOne(d => d.ClienteNavigation).WithMany(p => p.Envios)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("fk_envios_usuaris");
        });

        modelBuilder.Entity<EstatsEnvio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__estats_e__3213E83F5AE5AEC3");

            entity.ToTable("estats_envio");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .HasColumnName("nom");
        });

        modelBuilder.Entity<EstatsOferte>(entity =>
        {
            entity.ToTable("estats_ofertes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estat)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estat");
        });

        modelBuilder.Entity<FailedJob>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__failed_j__3213E83F7BA21DEB");

            entity.ToTable("failed_jobs");

            entity.HasIndex(e => e.Uuid, "failed_jobs_uuid_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Connection).HasColumnName("connection");
            entity.Property(e => e.Exception).HasColumnName("exception");
            entity.Property(e => e.FailedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("failed_at");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.Queue).HasColumnName("queue");
            entity.Property(e => e.Uuid)
                .HasMaxLength(255)
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<Incoterm>(entity =>
        {
            entity.ToTable("incoterms");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TipusIncontermId).HasColumnName("tipus_inconterm_id");
            entity.Property(e => e.TrackingStepsId).HasColumnName("tracking_steps_id");

            entity.HasOne(d => d.TipusInconterm).WithMany(p => p.Incoterms)
                .HasForeignKey(d => d.TipusIncontermId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_incoterms_tipus_incoterms");

            entity.HasOne(d => d.TrackingSteps).WithMany(p => p.Incoterms)
                .HasForeignKey(d => d.TrackingStepsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_incoterms_tracking_steps");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__jobs__3213E83F8D367E17");

            entity.ToTable("jobs");

            entity.HasIndex(e => e.Queue, "jobs_queue_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Attempts).HasColumnName("attempts");
            entity.Property(e => e.AvailableAt).HasColumnName("available_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.Queue)
                .HasMaxLength(255)
                .HasColumnName("queue");
            entity.Property(e => e.ReservedAt).HasColumnName("reserved_at");
        });

        modelBuilder.Entity<JobBatch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("job_batches_id_primary");

            entity.ToTable("job_batches");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.CancelledAt).HasColumnName("cancelled_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.FailedJobIds).HasColumnName("failed_job_ids");
            entity.Property(e => e.FailedJobs).HasColumnName("failed_jobs");
            entity.Property(e => e.FinishedAt).HasColumnName("finished_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Options).HasColumnName("options");
            entity.Property(e => e.PendingJobs).HasColumnName("pending_jobs");
            entity.Property(e => e.TotalJobs).HasColumnName("total_jobs");
        });

        modelBuilder.Entity<LiniesTransportMaritim>(entity =>
        {
            entity.ToTable("linies_transport_maritim");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CiutatId).HasColumnName("ciutat_id");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");

            entity.HasOne(d => d.Ciutat).WithMany(p => p.LiniesTransportMaritims)
                .HasForeignKey(d => d.CiutatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_linies_transport_maritim_ciutats");
        });

        modelBuilder.Entity<Migration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__migratio__3213E83F3B0FC53D");

            entity.ToTable("migrations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Batch).HasColumnName("batch");
            entity.Property(e => e.Migration1)
                .HasMaxLength(255)
                .HasColumnName("migration");
        });

        modelBuilder.Entity<Oferte>(entity =>
        {
            entity.ToTable("ofertes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AeroportDestiId).HasColumnName("aeroport_desti_id");
            entity.Property(e => e.AeroportOrigenId).HasColumnName("aeroport_origen_id");
            entity.Property(e => e.AgentComercialId).HasColumnName("agent_comercial_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Comentaris)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("comentaris");
            entity.Property(e => e.DataCreacio).HasColumnName("data_creacio");
            entity.Property(e => e.DataValidessaFina).HasColumnName("data_validessa_fina");
            entity.Property(e => e.DataValidessaInicial).HasColumnName("data_validessa_inicial");
            entity.Property(e => e.EstatEnvioId).HasColumnName("estat_envio_id");
            entity.Property(e => e.EstatOfertaId).HasColumnName("estat_oferta_id");
            entity.Property(e => e.IncotermId).HasColumnName("incoterm_id");
            entity.Property(e => e.LiniaTransportMaritimId).HasColumnName("linia_transport_maritim_id");
            entity.Property(e => e.OperadorId).HasColumnName("operador_id");
            entity.Property(e => e.PesBrut)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("pes_brut");
            entity.Property(e => e.PortDestiId).HasColumnName("port_desti_id");
            entity.Property(e => e.PortOrigenId).HasColumnName("port_origen_id");
            entity.Property(e => e.RaoRebuig)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rao_rebuig");
            entity.Property(e => e.TipusCarregaId).HasColumnName("tipus_carrega_id");
            entity.Property(e => e.TipusContenidorId).HasColumnName("tipus_contenidor_id");
            entity.Property(e => e.TipusFluxeId).HasColumnName("tipus_fluxe_id");
            entity.Property(e => e.TipusTransportId).HasColumnName("tipus_transport_id");
            entity.Property(e => e.TipusValidacioId).HasColumnName("tipus_validacio_id");
            entity.Property(e => e.TransportistaId).HasColumnName("transportista_id");
            entity.Property(e => e.Volum)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("volum");

            entity.HasOne(d => d.AeroportDesti).WithMany(p => p.OferteAeroportDestis)
                .HasForeignKey(d => d.AeroportDestiId)
                .HasConstraintName("FK_ofertes_aeroports1");

            entity.HasOne(d => d.AeroportOrigen).WithMany(p => p.OferteAeroportOrigens)
                .HasForeignKey(d => d.AeroportOrigenId)
                .HasConstraintName("FK_ofertes_aeroports");

            entity.HasOne(d => d.EstatEnvio).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.EstatEnvioId)
                .HasConstraintName("FK__ofertes__estat_e__3864608B");

            entity.HasOne(d => d.EstatOferta).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.EstatOfertaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ofertes_estats_ofertes");

            entity.HasOne(d => d.Incoterm).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.IncotermId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ofertes_incoterms");

            entity.HasOne(d => d.LiniaTransportMaritim).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.LiniaTransportMaritimId)
                .HasConstraintName("FK_ofertes_linies_transport_maritim");

            entity.HasOne(d => d.Operador).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.OperadorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ofertes_usuaris");

            entity.HasOne(d => d.PortDesti).WithMany(p => p.OfertePortDestis)
                .HasForeignKey(d => d.PortDestiId)
                .HasConstraintName("FK_ofertes_ports1");

            entity.HasOne(d => d.PortOrigen).WithMany(p => p.OfertePortOrigens)
                .HasForeignKey(d => d.PortOrigenId)
                .HasConstraintName("FK_ofertes_ports");

            entity.HasOne(d => d.TipusCarrega).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.TipusCarregaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ofertes_tipus_carrega");

            entity.HasOne(d => d.TipusContenidor).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.TipusContenidorId)
                .HasConstraintName("FK_ofertes_tipus_contenidors");

            entity.HasOne(d => d.TipusFluxe).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.TipusFluxeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ofertes_tipus_fluxes");

            entity.HasOne(d => d.TipusTransport).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.TipusTransportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ofertes_tipus_transports");

            entity.HasOne(d => d.TipusValidacio).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.TipusValidacioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ofertes_tipus_validacions");

            entity.HasOne(d => d.Transportista).WithMany(p => p.Ofertes)
                .HasForeignKey(d => d.TransportistaId)
                .HasConstraintName("FK_ofertes_transportistes");
        });

        modelBuilder.Entity<Paisso>(entity =>
        {
            entity.ToTable("paissos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");
        });

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("password_reset_tokens_email_primary");

            entity.ToTable("password_reset_tokens");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .HasColumnName("token");
        });

        modelBuilder.Entity<PersonalAccessToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__personal__3213E83FD03C6707");

            entity.ToTable("personal_access_tokens");

            entity.HasIndex(e => e.ExpiresAt, "personal_access_tokens_expires_at_index");

            entity.HasIndex(e => e.Token, "personal_access_tokens_token_unique").IsUnique();

            entity.HasIndex(e => new { e.TokenableType, e.TokenableId }, "personal_access_tokens_tokenable_type_tokenable_id_index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Abilities).HasColumnName("abilities");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime")
                .HasColumnName("expires_at");
            entity.Property(e => e.LastUsedAt)
                .HasColumnType("datetime")
                .HasColumnName("last_used_at");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Token)
                .HasMaxLength(64)
                .HasColumnName("token");
            entity.Property(e => e.TokenableId).HasColumnName("tokenable_id");
            entity.Property(e => e.TokenableType)
                .HasMaxLength(255)
                .HasColumnName("tokenable_type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Port>(entity =>
        {
            entity.ToTable("ports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CiutatId).HasColumnName("ciutat_id");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");

            entity.HasOne(d => d.Ciutat).WithMany(p => p.Ports)
                .HasForeignKey(d => d.CiutatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ports_ciutats");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("rols");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Rol1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rol");
        });

        modelBuilder.Entity<SeguimentOferte>(entity =>
        {
            entity.ToTable("seguiment_ofertes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataActualitzacio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("data_actualitzacio");
            entity.Property(e => e.EstatId)
                .HasDefaultValue(1)
                .HasColumnName("estat_id");
            entity.Property(e => e.OfertaId).HasColumnName("oferta_id");
            entity.Property(e => e.TrackingStepId).HasColumnName("tracking_step_id");

            entity.HasOne(d => d.Estat).WithMany(p => p.SeguimentOfertes)
                .HasForeignKey(d => d.EstatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_seguiment_ofertes_estats");

            entity.HasOne(d => d.Oferta).WithMany(p => p.SeguimentOfertes)
                .HasForeignKey(d => d.OfertaId)
                .HasConstraintName("FK_seguiment_ofertes_ofertes");

            entity.HasOne(d => d.TrackingStep).WithMany(p => p.SeguimentOfertes)
                .HasForeignKey(d => d.TrackingStepId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_seguiment_ofertes_steps");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sessions_id_primary");

            entity.ToTable("sessions");

            entity.HasIndex(e => e.LastActivity, "sessions_last_activity_index");

            entity.HasIndex(e => e.UserId, "sessions_user_id_index");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .HasColumnName("ip_address");
            entity.Property(e => e.LastActivity).HasColumnName("last_activity");
            entity.Property(e => e.Payload).HasColumnName("payload");
            entity.Property(e => e.UserAgent).HasColumnName("user_agent");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<TipusCarrega>(entity =>
        {
            entity.ToTable("tipus_carrega");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tipus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipus");
        });

        modelBuilder.Entity<TipusContenidor>(entity =>
        {
            entity.ToTable("tipus_contenidors");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tipus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipus");
        });

        modelBuilder.Entity<TipusFlux>(entity =>
        {
            entity.ToTable("tipus_fluxes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tipus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipus");
        });

        modelBuilder.Entity<TipusIncoterm>(entity =>
        {
            entity.ToTable("tipus_incoterms");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Codi)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("codi");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");
        });

        modelBuilder.Entity<TipusTransport>(entity =>
        {
            entity.ToTable("tipus_transports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tipus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipus");
        });

        modelBuilder.Entity<TipusValidacion>(entity =>
        {
            entity.ToTable("tipus_validacions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tipus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipus");
        });

        modelBuilder.Entity<TrackingStep>(entity =>
        {
            entity.ToTable("tracking_steps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.Ordre).HasColumnName("ordre");
        });

        modelBuilder.Entity<Transportiste>(entity =>
        {
            entity.ToTable("transportistes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CiutatId).HasColumnName("ciutat_id");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");

            entity.HasOne(d => d.Ciutat).WithMany(p => p.Transportistes)
                .HasForeignKey(d => d.CiutatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transportistes_ciutats");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F956F077B");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.EmailVerifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("email_verified_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RememberToken)
                .HasMaxLength(100)
                .HasColumnName("remember_token");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Usuari>(entity =>
        {
            entity.ToTable("usuaris");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Actiu)
                .IsRequired()
                .HasDefaultValueSql("('1')")
                .HasColumnName("actiu");
            entity.Property(e => e.Cif)
                .HasMaxLength(20)
                .HasColumnName("cif");
            entity.Property(e => e.Cognoms)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cognoms");
            entity.Property(e => e.Contrasenya)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("contrasenya");
            entity.Property(e => e.Correu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correu");
            entity.Property(e => e.DniFoto).HasColumnName("dni_foto");
            entity.Property(e => e.Empresa)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("empresa");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.RolId).HasColumnName("rol_id");
            entity.Property(e => e.Telefon)
                .HasMaxLength(20)
                .HasColumnName("telefon");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuaris)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usuaris_rols");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
