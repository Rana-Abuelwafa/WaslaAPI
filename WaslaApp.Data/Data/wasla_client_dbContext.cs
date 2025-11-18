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

    public virtual DbSet<ApplyTax> ApplyTaxes { get; set; }

    public virtual DbSet<ClientBrand> ClientBrands { get; set; }

    public virtual DbSet<ClientCopoun> ClientCopouns { get; set; }

    public virtual DbSet<ClientImage> ClientImages { get; set; }

    public virtual DbSet<ClientProfile> ClientProfiles { get; set; }

    public virtual DbSet<ClientService> ClientServices { get; set; }

    public virtual DbSet<InvoiceMain> InvoiceMains { get; set; }

    public virtual DbSet<Main_RegistrationQuestion> Main_RegistrationQuestions { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<RegistrationAnswer> RegistrationAnswers { get; set; }

    public virtual DbSet<RegistrationQuestions_Translation> RegistrationQuestions_Translations { get; set; }

    public virtual DbSet<audit_log> audit_logs { get; set; }

    public virtual DbSet<clientinvoiceswithdetail> clientinvoiceswithdetails { get; set; }

    public virtual DbSet<features_translation> features_translations { get; set; }

    public virtual DbSet<featureswithtranslation> featureswithtranslations { get; set; }

    public virtual DbSet<main_feature> main_features { get; set; }

    public virtual DbSet<main_service> main_services { get; set; }

    public virtual DbSet<package> packages { get; set; }

    public virtual DbSet<package_translation> package_translations { get; set; }

    public virtual DbSet<packages_feature> packages_features { get; set; }

    public virtual DbSet<packagesdetailswithservice> packagesdetailswithservices { get; set; }

    public virtual DbSet<registrationqueswithlang> registrationqueswithlangs { get; set; }

    public virtual DbSet<reports_main> reports_mains { get; set; }

    public virtual DbSet<service_package> service_packages { get; set; }

    public virtual DbSet<service_package_price> service_package_prices { get; set; }

    public virtual DbSet<service_translation> service_translations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:5432;Database=wasla_client_db;Username=postgres;Password=Berlin2020");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplyTax>(entity =>
        {
            entity.HasKey(e => e.tax_id).HasName("ApplyTax_pkey");

            entity.ToTable("ApplyTax");

            entity.Property(e => e.tax_id).ValueGeneratedNever();
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.tax_code).HasMaxLength(20);
            entity.Property(e => e.tax_name).HasMaxLength(50);
            entity.Property(e => e.tax_sign).HasColumnType("char");
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<ClientBrand>(entity =>
        {
            entity.HasKey(e => e.id).HasName("ClientBrand_pkey");

            entity.ToTable("ClientBrand");

            entity.Property(e => e.brand_name).HasMaxLength(100);
            entity.Property(e => e.brand_type).HasMaxLength(20);
            entity.Property(e => e.client_Id).HasColumnType("character varying");
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<ClientCopoun>(entity =>
        {
            entity.HasKey(e => e.id).HasName("ClientCopouns_pkey");

            entity.Property(e => e.client_id).HasColumnType("character varying");
            entity.Property(e => e.copoun).HasMaxLength(50);
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.discount_type).HasComment("1 = percentage\n2 = amount");
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<ClientImage>(entity =>
        {
            entity.HasKey(e => e.id).HasName("ClientImages_pkey");

            entity.Property(e => e.client_id).HasColumnType("character varying");
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.img_name).HasMaxLength(50);
            entity.Property(e => e.img_path).HasMaxLength(100);
            entity.Property(e => e.type).HasComment("1 for profile");
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<ClientProfile>(entity =>
        {
            entity.HasKey(e => e.profile_id).HasName("ClientProfile_pkey");

            entity.ToTable("ClientProfile");

            entity.Property(e => e.client_email).HasMaxLength(50);
            entity.Property(e => e.client_id).HasColumnType("character varying");
            entity.Property(e => e.client_name).HasMaxLength(50);
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.fb_link).HasMaxLength(50);
            entity.Property(e => e.gender).HasMaxLength(50);
            entity.Property(e => e.lang).HasMaxLength(20);
            entity.Property(e => e.nation).HasMaxLength(50);
            entity.Property(e => e.pay_code).HasMaxLength(20);
            entity.Property(e => e.phone_number).HasMaxLength(50);
            entity.Property(e => e.twitter_link).HasMaxLength(50);
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<ClientService>(entity =>
        {
            entity.HasKey(e => e.id).HasName("ClientServices_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("nextval('\"ClientServices_id_seq\"'::regclass)");
            entity.Property(e => e.active).HasDefaultValue(false);
            entity.Property(e => e.client_id).HasColumnType("character varying");
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.service_package_id).HasDefaultValue(0);
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<InvoiceMain>(entity =>
        {
            entity.HasKey(e => e.invoice_id).HasName("InvoiceMain_pkey");

            entity.ToTable("InvoiceMain");

            entity.Property(e => e.client_email).HasMaxLength(50);
            entity.Property(e => e.client_id).HasColumnType("character varying");
            entity.Property(e => e.client_name).HasMaxLength(100);
            entity.Property(e => e.copoun_id).HasDefaultValueSql("0");
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.curr_code).HasMaxLength(20);
            entity.Property(e => e.grand_total_price).HasDefaultValueSql("0");
            entity.Property(e => e.invoice_code).HasMaxLength(50);
            entity.Property(e => e.invoice_code_auto).HasMaxLength(20);
            entity.Property(e => e.invoice_date).HasColumnType("timestamp without time zone");
            entity.Property(e => e.status).HasComment("1 = pending\n2=checkout\n3=payed");
            entity.Property(e => e.tax_id).HasDefaultValue(0);
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<Main_RegistrationQuestion>(entity =>
        {
            entity.HasKey(e => e.ques_id).HasName("Main_RegistrationQuestions_pkey");

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.ques_type).HasMaxLength(20);
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.pay_id).HasName("PaymentMethods_pkey");

            entity.Property(e => e.pay_id).ValueGeneratedNever();
            entity.Property(e => e.pay_code).HasMaxLength(20);
            entity.Property(e => e.pay_name).HasMaxLength(50);
        });

        modelBuilder.Entity<RegistrationAnswer>(entity =>
        {
            entity.HasKey(e => e.id).HasName("RegistrationAnswers_pkey");

            entity.Property(e => e.client_id).HasColumnType("character varying");
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.lang_code).HasMaxLength(20);
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<RegistrationQuestions_Translation>(entity =>
        {
            entity.HasKey(e => e.id).HasName("RegistrationQuestions_Translations_pkey");

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.lang_code).HasMaxLength(20);
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<audit_log>(entity =>
        {
            entity.HasKey(e => e.id).HasName("audit_log_pkey");

            entity.ToTable("audit_log");

            entity.Property(e => e.changed_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.changed_by).HasDefaultValueSql("CURRENT_USER");
        });

        modelBuilder.Entity<clientinvoiceswithdetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("clientinvoiceswithdetails");

            entity.Property(e => e.client_email).HasMaxLength(50);
            entity.Property(e => e.client_id).HasColumnType("character varying");
            entity.Property(e => e.client_name).HasMaxLength(100);
            entity.Property(e => e.copoun).HasMaxLength(50);
            entity.Property(e => e.curr_code).HasMaxLength(20);
            entity.Property(e => e.invoice_code).HasMaxLength(50);
            entity.Property(e => e.invoice_code_auto).HasMaxLength(20);
            entity.Property(e => e.invoice_date).HasColumnType("timestamp without time zone");
            entity.Property(e => e.tax_code).HasMaxLength(20);
            entity.Property(e => e.tax_name).HasMaxLength(50);
            entity.Property(e => e.tax_sign).HasColumnType("char");
        });

        modelBuilder.Entity<features_translation>(entity =>
        {
            entity.HasKey(e => e.id).HasName("features_translation_pkey");

            entity.ToTable("features_translation");

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.lang_code).HasMaxLength(5);
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<featureswithtranslation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("featureswithtranslations");

            entity.Property(e => e.feature_code).HasMaxLength(20);
            entity.Property(e => e.lang_code).HasMaxLength(5);
        });

        modelBuilder.Entity<main_feature>(entity =>
        {
            entity.HasKey(e => e.id).HasName("packages_features_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("nextval('packages_features_id_seq'::regclass)");
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.feature_code).HasMaxLength(20);
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<main_service>(entity =>
        {
            entity.HasKey(e => e.id).HasName("main_services_pkey");

            entity.HasIndex(e => new { e.service_code, e.active }, "main_services_service_code_key").IsUnique();

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.price_calc_type)
                .HasDefaultValue((short)1)
                .HasComment("1 = monthly\n2 = yearly\n3 = per project");
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<package>(entity =>
        {
            entity.HasKey(e => e.id).HasName("packages_pkey");

            entity.HasIndex(e => new { e.package_code, e.active }, "packages_package_code_key").IsUnique();

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.order).HasDefaultValue((short)0);
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<package_translation>(entity =>
        {
            entity.HasKey(e => e.id).HasName("package_translations_pkey");

            entity.HasIndex(e => e.lang_code, "idx_package_translations_lang");

            entity.HasIndex(e => new { e.package_id, e.lang_code }, "package_translations_package_id_lang_code_key").IsUnique();

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.lang_code).HasMaxLength(5);
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.package).WithMany(p => p.package_translations)
                .HasForeignKey(d => d.package_id)
                .HasConstraintName("package_translations_package_id_fkey");
        });

        modelBuilder.Entity<packages_feature>(entity =>
        {
            entity.HasKey(e => e.id).HasName("packages_features_pkey1");

            entity.Property(e => e.id).HasDefaultValueSql("nextval('packages_features_id_seq1'::regclass)");
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<packagesdetailswithservice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("packagesdetailswithservices");

            entity.Property(e => e.curr_code).HasMaxLength(3);
            entity.Property(e => e.discount_amount).HasPrecision(10, 2);
            entity.Property(e => e.lang_code).HasMaxLength(5);
            entity.Property(e => e.package_price).HasPrecision(10, 2);
            entity.Property(e => e.package_sale_price).HasPrecision(10, 2);
        });

        modelBuilder.Entity<registrationqueswithlang>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("registrationqueswithlangs");

            entity.Property(e => e.lang_code).HasMaxLength(20);
            entity.Property(e => e.ques_type).HasMaxLength(20);
        });

        modelBuilder.Entity<reports_main>(entity =>
        {
            entity.HasKey(e => e.id).HasName("reports_main_pkey");

            entity.ToTable("reports_main");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.report_code).HasMaxLength(20);
            entity.Property(e => e.report_name).HasMaxLength(100);
        });

        modelBuilder.Entity<service_package>(entity =>
        {
            entity.HasKey(e => e.id).HasName("service_packages_pkey");

            entity.HasIndex(e => new { e.service_id, e.package_id }, "service_packages_service_id_package_id_key").IsUnique();

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.is_recommend).HasDefaultValue(false);
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.package).WithMany(p => p.service_packages)
                .HasForeignKey(d => d.package_id)
                .HasConstraintName("service_packages_package_id_fkey");

            entity.HasOne(d => d.service).WithMany(p => p.service_packages)
                .HasForeignKey(d => d.service_id)
                .HasConstraintName("service_packages_service_id_fkey");
        });

        modelBuilder.Entity<service_package_price>(entity =>
        {
            entity.HasKey(e => e.id).HasName("service_package_prices_pkey");

            entity.HasIndex(e => e.curr_code, "idx_prices_currency");

            entity.HasIndex(e => new { e.service_package_id, e.curr_code }, "service_package_prices_service_package_id_curr_code_key").IsUnique();

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.curr_code).HasMaxLength(3);
            entity.Property(e => e.discount_amount).HasPrecision(10, 2);
            entity.Property(e => e.package_price).HasPrecision(10, 2);
            entity.Property(e => e.package_sale_price).HasPrecision(10, 2);
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.service_package).WithMany(p => p.service_package_prices)
                .HasForeignKey(d => d.service_package_id)
                .HasConstraintName("service_package_prices_service_package_id_fkey");
        });

        modelBuilder.Entity<service_translation>(entity =>
        {
            entity.HasKey(e => e.id).HasName("service_translations_pkey");

            entity.HasIndex(e => e.lang_code, "idx_service_translations_lang");

            entity.HasIndex(e => new { e.service_id, e.lang_code }, "service_translations_service_id_lang_code_key").IsUnique();

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.created_by).HasMaxLength(100);
            entity.Property(e => e.lang_code).HasMaxLength(5);
            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.service).WithMany(p => p.service_translations)
                .HasForeignKey(d => d.service_id)
                .HasConstraintName("service_translations_service_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
