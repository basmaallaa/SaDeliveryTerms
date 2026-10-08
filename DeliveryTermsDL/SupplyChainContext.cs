using System;
using System.Collections.Generic;
using DeliveryTermsDL.Models.SupplyChain;
using Microsoft.EntityFrameworkCore;

namespace DeliveryTermsDL;

public partial class SupplyChainContext : DbContext
{
    public SupplyChainContext()
    {
    }

    public SupplyChainContext(DbContextOptions<SupplyChainContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SaDeliveryTerm> SaDeliveryTerms { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<SaDeliveryTerm>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("SA_DELIVERY_TERMS_PK");

            entity.ToTable("SA_DELIVERY_TERMS");

            entity.Property(e => e.Code)
                .HasPrecision(3)
                .HasColumnName("CODE");
            entity.Property(e => e.ActiveFlag)
                .HasDefaultValueSql("1")
                .HasColumnType("NUMBER(1)")
                .HasColumnName("ACTIVE_FLAG");
            entity.Property(e => e.BName)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("B_NAME");
            entity.Property(e => e.ChangeDate)
                .HasColumnType("DATE")
                .HasColumnName("CHANGE_DATE");
            entity.Property(e => e.ChangeUser)
                .HasPrecision(10)
                .HasColumnName("CHANGE_USER");
            entity.Property(e => e.Days)
                .HasPrecision(4)
                .HasColumnName("DAYS");
            entity.Property(e => e.EntryDate)
                .HasColumnType("DATE")
                .HasColumnName("ENTRY_DATE");
            entity.Property(e => e.EntryUser)
                .HasPrecision(10)
                .HasColumnName("ENTRY_USER");
            entity.Property(e => e.SName)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("S_NAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
