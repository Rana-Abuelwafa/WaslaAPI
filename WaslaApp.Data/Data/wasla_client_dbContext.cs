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

    public virtual DbSet<ClientBrand> ClientBrands { get; set; }

    public virtual DbSet<ClientCopoun> ClientCopouns { get; set; }

    public virtual DbSet<ClientImage> ClientImages { get; set; }

    public virtual DbSet<ClientProfile> ClientProfiles { get; set; }

    public virtual DbSet<ClientService> ClientServices { get; set; }

    public virtual DbSet<MailTemp> MailTemps { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<RegistrationAnswer> RegistrationAnswers { get; set; }

    public virtual DbSet<RegistrationQuestion> RegistrationQuestions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:5432;Database=wasla_client_db;Username=postgres;Password=Berlin2020");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientBrand>(entity =>
        {
            entity.HasKey(e => e.id).HasName("ClientBrand_pkey");

            entity.ToTable("ClientBrand");

            entity.Property(e => e.brand_name).HasMaxLength(100);
            entity.Property(e => e.brand_type).HasMaxLength(20);
            entity.Property(e => e.client_Id).HasColumnType("character varying");
        });

        modelBuilder.Entity<ClientCopoun>(entity =>
        {
            entity.HasKey(e => e.id).HasName("ClientCopouns_pkey");

            entity.Property(e => e.client_id).HasColumnType("character varying");
            entity.Property(e => e.copoun).HasMaxLength(50);
        });

        modelBuilder.Entity<ClientImage>(entity =>
        {
            entity.HasKey(e => e.id).HasName("ClientImages_pkey");

            entity.Property(e => e.client_id).HasColumnType("character varying");
            entity.Property(e => e.img_name).HasMaxLength(50);
            entity.Property(e => e.img_path).HasMaxLength(100);
            entity.Property(e => e.type).HasComment("1 for profile");
        });

        modelBuilder.Entity<ClientProfile>(entity =>
        {
            entity.HasKey(e => e.profile_id).HasName("ClientProfile_pkey");

            entity.ToTable("ClientProfile");

            entity.Property(e => e.client_email).HasMaxLength(50);
            entity.Property(e => e.client_id).HasColumnType("character varying");
            entity.Property(e => e.client_name).HasMaxLength(50);
            entity.Property(e => e.fb_link).HasMaxLength(50);
            entity.Property(e => e.gender).HasMaxLength(50);
            entity.Property(e => e.lang).HasMaxLength(20);
            entity.Property(e => e.nation).HasMaxLength(50);
            entity.Property(e => e.pay_code).HasMaxLength(20);
            entity.Property(e => e.phone_number).HasMaxLength(50);
            entity.Property(e => e.twitter_link).HasMaxLength(50);
        });

        modelBuilder.Entity<ClientService>(entity =>
        {
            entity.HasKey(e => e.id).HasName("ClientServices_pkey");

            entity.Property(e => e.client_id).HasColumnType("character varying");
        });

        modelBuilder.Entity<MailTemp>(entity =>
        {
            entity.HasKey(e => e.id).HasName("MailTemps_pkey");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.lang).HasMaxLength(20);
            entity.Property(e => e.mail_Subject).HasMaxLength(50);
            entity.Property(e => e.type).HasComment("1 => for mail Confirmation\n2 => for otp verify");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.pay_id).HasName("PaymentMethods_pkey");

            entity.Property(e => e.pay_id).ValueGeneratedNever();
            entity.Property(e => e.pay_code).HasMaxLength(20);
            entity.Property(e => e.pay_name).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.productId).HasName("Product_pkey");

            entity.ToTable("Product");

            entity.Property(e => e.productId).ValueGeneratedNever();
        });

        modelBuilder.Entity<RegistrationAnswer>(entity =>
        {
            entity.HasKey(e => e.id).HasName("RegistrationAnswers_pkey");

            entity.Property(e => e.client_id).HasColumnType("character varying");
            entity.Property(e => e.lang_code).HasMaxLength(20);
        });

        modelBuilder.Entity<RegistrationQuestion>(entity =>
        {
            entity.HasKey(e => e.ques_id).HasName("RegistrationQuestions_pkey");

            entity.Property(e => e.lang_code).HasMaxLength(20);
            entity.Property(e => e.order).HasDefaultValue(0);
            entity.Property(e => e.ques_type).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
