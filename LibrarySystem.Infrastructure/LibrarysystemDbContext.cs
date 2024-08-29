using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using LibrarySystem.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure;

public partial class LibrarysystemDbContext : IdentityDbContext<AppUser>
{
    public LibrarysystemDbContext(DbContextOptions<LibrarysystemDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Borrow> Borrows { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=PostgreSQLConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.IdBook).HasName("book_pkey");

            entity.ToTable("book");

            entity.HasIndex(e => e.Isbn, "book_isbn_key").IsUnique();

            entity.Property(e => e.IdBook).HasColumnName("id_book");
            entity.Property(e => e.Author)
                .HasMaxLength(255)
                .HasColumnName("author");
            entity.Property(e => e.Availablebook)
                .HasDefaultValue(0)
                .HasColumnName("availablebook");
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .HasColumnName("category");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("isbn");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.Publisher)
                .HasMaxLength(255)
                .HasColumnName("publisher");
            entity.Property(e => e.Purchasedate).HasColumnName("purchasedate");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Language)
                .HasMaxLength(100)
                .HasColumnName("language");
        });

        modelBuilder.Entity<Borrow>(entity =>
        {
            entity.HasKey(e => e.IdBorrow).HasName("borrow_pkey");

            entity.ToTable("borrow");

            entity.Property(e => e.IdBorrow).HasColumnName("id_borrow");
            entity.Property(e => e.DateBorrow).HasColumnName("date_borrow");
            entity.Property(e => e.DateReturn).HasColumnName("date_return");
            entity.Property(e => e.IdBook).HasColumnName("id_book");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Penalty)
                .HasPrecision(10, 2)
                .HasColumnName("penalty");

            entity.HasOne(d => d.IdBookNavigation).WithMany(p => p.Borrows)
                .HasForeignKey(d => d.IdBook)
                .HasConstraintName("borrow_id_book_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Borrows)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("borrow_id_user_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("user_pkey");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "user_email_key").IsUnique();

            entity.HasIndex(e => e.Librarycard, "user_librarycard_key").IsUnique();

            entity.Property(e => e.IdUser)
            .HasColumnName("id_user")
            .ValueGeneratedOnAdd();
            entity.Property(e => e.Cardexp).HasColumnName("cardexp");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Fname)
                .HasMaxLength(100)
                .HasColumnName("fname");
            entity.Property(e => e.Librarycard)
                .HasMaxLength(50)
                .HasColumnName("librarycard");
            entity.Property(e => e.Lname)
                .HasMaxLength(100)
                .HasColumnName("lname");
            entity.Property(e => e.Notreturnbook)
                .HasDefaultValue(0)
                .HasColumnName("notreturnbook");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Position)
                .HasMaxLength(50) 
                .HasColumnName("position");
            entity.Property(e => e.Penalty)
                .HasMaxLength(50)
                .HasColumnName("penalty");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
