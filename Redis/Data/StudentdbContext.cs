using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Redis.Models;

namespace Redis.Data;

public partial class StudentdbContext : DbContext
{
    public StudentdbContext()
    {
    }

    public StudentdbContext(DbContextOptions<StudentdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=Studentdb;Username=postgres;Password=Root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("customer_pkey");

            entity.ToTable("customer");

            entity.Property(e => e.Customerid)
                .ValueGeneratedNever()
                .HasColumnName("customerid");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.Customername)
                .HasMaxLength(50)
                .HasColumnName("customername");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Phone).HasColumnName("phone");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("students_pkey");

            entity.ToTable("students");

            entity.Property(e => e.StudentId)
                .ValueGeneratedNever()
                .HasColumnName("student_id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Major)
                .HasMaxLength(50)
                .HasColumnName("major");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
