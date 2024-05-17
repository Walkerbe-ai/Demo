using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace TechnoService.Data.Models;

public partial class DemoContext : DbContext
{
    public DemoContext()
    {
    }

    public DemoContext(DbContextOptions<DemoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CatalogOrder> CatalogOrders { get; set; }

    public virtual DbSet<Human> Humans { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<TypeEquipment> TypeEquipments { get; set; }

    public virtual DbSet<TypeProblem> TypeProblems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=9sQecrJCyhh;database=demo", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<CatalogOrder>(entity =>
        {
            entity.HasKey(e => e.IdCatalogOrders).HasName("PRIMARY");

            entity.ToTable("catalog_orders");

            entity.HasIndex(e => e.IdEmployee, "FK_id_employee_idx");

            entity.HasIndex(e => e.IdOrders, "FK_id_orders_idx");

            entity.Property(e => e.IdCatalogOrders).HasColumnName("id_catalog_orders");
            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");
            entity.Property(e => e.IdOrders).HasColumnName("id_orders");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.CatalogOrders)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("FK_id_employee");

            entity.HasOne(d => d.IdOrdersNavigation).WithMany(p => p.CatalogOrders)
                .HasForeignKey(d => d.IdOrders)
                .HasConstraintName("FK_id_orders");
        });

        modelBuilder.Entity<Human>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("human");

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.FullnameUser)
                .HasMaxLength(100)
                .HasColumnName("fullname_user");
            entity.Property(e => e.Login)
                .HasMaxLength(20)
                .HasColumnName("login");
            entity.Property(e => e.PasswordUser)
                .HasMaxLength(64)
                .HasColumnName("password_user");
            entity.Property(e => e.RoleUser)
                .HasMaxLength(45)
                .HasColumnName("role_user");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdApplication).HasName("PRIMARY");

            entity.ToTable("order");

            entity.HasIndex(e => e.WorkStatus, "Fk_IdStageWork_idx");

            entity.HasIndex(e => e.IdTypeEquipment, "Fk_id_type_equipment_idx");

            entity.HasIndex(e => e.IdTypeProblem, "Fk_id_type_problem_idx");

            entity.Property(e => e.IdApplication).HasColumnName("id_application");
            entity.Property(e => e.CommentApplication)
                .HasColumnType("text")
                .HasColumnName("comment_application");
            entity.Property(e => e.Cost)
                .HasPrecision(10, 2)
                .HasColumnName("cost");
            entity.Property(e => e.DateAddition).HasColumnName("date_addition");
            entity.Property(e => e.DateEnd).HasColumnName("date_end");
            entity.Property(e => e.DescriptionApplication)
                .HasColumnType("text")
                .HasColumnName("description_application");
            entity.Property(e => e.IdTypeEquipment).HasColumnName("id_type_equipment");
            entity.Property(e => e.IdTypeProblem).HasColumnName("id_type_problem");
            entity.Property(e => e.NameClient)
                .HasMaxLength(45)
                .HasColumnName("name_client");
            entity.Property(e => e.NameEquipment)
                .HasMaxLength(45)
                .HasColumnName("name_equipment");
            entity.Property(e => e.PeriodExecution)
                .HasComment("предварительная дата завершения")
                .HasColumnName("period_execution");
            entity.Property(e => e.SerialNumber)
                .HasComment("серийный номер")
                .HasColumnName("serial_number");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");
            entity.Property(e => e.TimeWork)
                .HasColumnType("time")
                .HasColumnName("time_work");
            entity.Property(e => e.WorkStatus)
                .HasMaxLength(45)
                .HasColumnName("work_status");

            entity.HasOne(d => d.IdTypeEquipmentNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdTypeEquipment)
                .HasConstraintName("Fk_id_type_equipment");

            entity.HasOne(d => d.IdTypeProblemNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdTypeProblem)
                .HasConstraintName("Fk_id_type_problem");
        });

        modelBuilder.Entity<TypeEquipment>(entity =>
        {
            entity.HasKey(e => e.IdTypeEquipment).HasName("PRIMARY");

            entity.ToTable("type_equipment");

            entity.Property(e => e.IdTypeEquipment)
                .ValueGeneratedNever()
                .HasColumnName("id_type_equipment");
            entity.Property(e => e.NameTypeEquipment)
                .HasMaxLength(45)
                .HasColumnName("name_type_equipment");
        });

        modelBuilder.Entity<TypeProblem>(entity =>
        {
            entity.HasKey(e => e.IdTypeProblem).HasName("PRIMARY");

            entity.ToTable("type_problem");

            entity.Property(e => e.IdTypeProblem)
                .HasComment("тип неисправностей")
                .HasColumnName("id_type_problem");
            entity.Property(e => e.NameTypeProblem)
                .HasMaxLength(45)
                .HasColumnName("name_type_problem");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
