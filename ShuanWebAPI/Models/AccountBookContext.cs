using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShuanWebAPI.Models;

public partial class AccountBookContext : DbContext
{
    public AccountBookContext(DbContextOptions<AccountBookContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accountbookfunction> Accountbookfunction { get; set; }

    public virtual DbSet<Bank> Bank { get; set; }

    public virtual DbSet<Category> Category { get; set; }

    public virtual DbSet<DailyExpense> DailyExpense { get; set; }

    public virtual DbSet<Group> Group { get; set; }

    public virtual DbSet<Item> Item { get; set; }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<Todo> Todo { get; set; }

    public virtual DbSet<UploadFile> UploadFile { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accountbookfunction>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AccountbookfunctionID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.FunctionName).HasMaxLength(50);
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.BankID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BankName).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CategoryID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<DailyExpense>(entity =>
        {
            entity.Property(e => e.ID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Bank).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Item).HasMaxLength(50);
            entity.Property(e => e.RecordDateTime).HasColumnType("datetime");
            entity.Property(e => e.Remark).HasMaxLength(50);
            entity.Property(e => e.UpdateDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.Property(e => e.GroupId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.ItemID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ItemName).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Todo>(entity =>
        {
            entity.Property(e => e.TodoId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccountId).HasMaxLength(50);
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GroupId).HasMaxLength(50);
            entity.Property(e => e.Thing).HasMaxLength(50);
        });

        modelBuilder.Entity<UploadFile>(entity =>
        {
            entity.HasKey(e => e.UploadFileID).HasName("PK__UploadFi__6819F4CE1561F687");

            entity.Property(e => e.UploadFileID).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IDNavigation).WithMany(p => p.UploadFile)
                .HasForeignKey(d => d.ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_File_ToDailyExpense");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C948265FA");

            entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
