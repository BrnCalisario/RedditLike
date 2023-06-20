using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Reddit.Model;

public partial class RedditContext : DbContext
{
    public RedditContext()
    {
    }

    public RedditContext(DbContextOptions<RedditContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Upvote> Upvotes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=CT-C-0018A\\SQLEXPRESS;Initial Catalog=Reddit;Integrated Security=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Group__3214EC273A86452B");

            entity.ToTable("Group");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Image).HasColumnType("image");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");

            entity.HasOne(d => d.Owner).WithMany(p => p.Groups)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK__Group__OwnerID__398D8EEE");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Post__3214EC27575B8234");

            entity.ToTable("Post");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.IndexedImage).HasColumnType("image");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Author).WithMany(p => p.Posts)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Post__AuthorID__3E52440B");

            entity.HasOne(d => d.Group).WithMany(p => p.Posts)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Post__GroupID__3F466844");
        });

        modelBuilder.Entity<Upvote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Upvote__3214EC27EFFFBE1C");

            entity.ToTable("Upvote");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Value).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Post).WithMany(p => p.Upvotes)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Upvote__PostID__4316F928");

            entity.HasOne(d => d.User).WithMany(p => p.Upvotes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Upvote__UserID__4222D4EF");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC2711CC5D48");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.ProfilePicture).HasColumnType("image");
            entity.Property(e => e.Salt)
                .IsRequired()
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("SALT");
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserGrou__3214EC27D576ED40");

            entity.ToTable("UserGroup");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
