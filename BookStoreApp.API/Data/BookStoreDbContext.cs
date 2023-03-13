using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Data;

public partial class BookStoreDbContext : IdentityDbContext<APIUser>
{
    public BookStoreDbContext()
    {
    }

    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=twoscreendell\\sqlsvr2017; Database=BookStoreDb; Trusted_Connection=True; Trust Server Certificate=true; MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authors__3214EC075BEE3A0D");

            entity.Property(e => e.Bio).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC0725FB5414");

            entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EAD99975F6").IsUnique();

            entity.Property(e => e.Image).HasMaxLength(50);
            entity.Property(e => e.Isbn)
                .HasMaxLength(50)
                .HasColumnName("ISBN");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Summary).HasMaxLength(250);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("TItle");

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_Books_ToTable");
        });

        // Seed the API roles:
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
				Id             = "a0075345-4251-4797-9d12-465d5391d554",
				Name           = "User",
                NormalizedName = "USER"
			},
			new IdentityRole
			{
				Id             = "70519e86-dc76-4851-acc1-7329dc8e45cd",
				Name           = "Admin",
				NormalizedName = "ADMIN"
			}

		);

        var hasher = new PasswordHasher<APIUser>();

        // Seed the API Users:
        modelBuilder.Entity<APIUser>().HasData(
            new APIUser
			{
                Id                 = "f2b62630-46cd-44e6-938d-343704f5b4e1",
                Email              = "admin@bookstore.com",
                NormalizedEmail    = "ADMIN@BOOKSTORE.COM",
				UserName           = "admin@bookstore.com",
				NormalizedUserName = "ADMIN@BOOKSTORE.COM",
                FirstName          = "System",
                LastName           = "Admin",
                PasswordHash       = hasher.HashPassword(null, "P@ssword1")
			},
			new APIUser
            {
                Id                 = "ecee591a-54d7-4c6c-a4bb-8bae64f0db29",
                Email              = "user@bookstore.com",
                NormalizedEmail    = "USER@BOOKSTORE.COM",
                UserName           = "user@bookstore.com",
                NormalizedUserName = "USER@BOOKSTORE.COM",
                FirstName          = "API",
                LastName           = "User",
                PasswordHash       = hasher.HashPassword(null, "P@ssword1")
            }
		);

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "a0075345-4251-4797-9d12-465d5391d554",
                UserId = "ecee591a-54d7-4c6c-a4bb-8bae64f0db29"
			},
			new IdentityUserRole<string>
			{
				RoleId = "70519e86-dc76-4851-acc1-7329dc8e45cd",
				UserId = "f2b62630-46cd-44e6-938d-343704f5b4e1"
			}

			);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
