using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Data;

public partial class wasla_client_dbContext : DbContext
{
    public wasla_client_dbContext()
    {
    }

    public wasla_client_dbContext(DbContextOptions<wasla_client_dbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RegistrationAnswer> RegistrationAnswers { get; set; }

    public virtual DbSet<RegistrationQuestion> RegistrationQuestions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:5432;Database=wasla_client_db;Username=postgres;Password=123456");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RegistrationAnswer>(entity =>
        {
            entity.HasKey(e => e.id).HasName("RegistrationAnswers_pkey");

            entity.Property(e => e.lang_code).HasMaxLength(20);
        });

        modelBuilder.Entity<RegistrationQuestion>(entity =>
        {
            entity.HasKey(e => e.ques_id).HasName("RegistrationQuestions_pkey");

            entity.Property(e => e.lang_code).HasMaxLength(20);
            entity.Property(e => e.ques_type).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
