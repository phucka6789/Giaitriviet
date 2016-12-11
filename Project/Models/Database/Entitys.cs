namespace Project.Models.Database
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Entitys : DbContext
    {
        public Entitys()
            : base("name=Entitys")
        {
        }

        public virtual DbSet<Administrator> Administrators { get; set; }
        public virtual DbSet<Business> Businesses { get; set; }
        public virtual DbSet<CatNew> CatNews { get; set; }
        public virtual DbSet<CatVideo> CatVideos { get; set; }
        public virtual DbSet<ContenNew> ContenNews { get; set; }
        public virtual DbSet<GrantPermission> GrantPermissions { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Video> Videos { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrator>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Administrator>()
                .HasMany(e => e.CatNews)
                .WithOptional(e => e.Administrator)
                .HasForeignKey(e => e.CreatedBy);

            modelBuilder.Entity<Administrator>()
                .HasMany(e => e.CatNews1)
                .WithOptional(e => e.Administrator1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<Administrator>()
                .HasMany(e => e.CatVideos)
                .WithOptional(e => e.Administrator)
                .HasForeignKey(e => e.CreatedBy);

            modelBuilder.Entity<Administrator>()
                .HasMany(e => e.CatVideos1)
                .WithOptional(e => e.Administrator1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<Administrator>()
                .HasMany(e => e.ContenNews)
                .WithOptional(e => e.Administrator)
                .HasForeignKey(e => e.CreatedBy);

            modelBuilder.Entity<Administrator>()
                .HasMany(e => e.ContenNews1)
                .WithOptional(e => e.Administrator1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<Administrator>()
                .HasMany(e => e.GrantPermissions)
                .WithRequired(e => e.Administrator)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Administrator>()
                .HasMany(e => e.Videos)
                .WithOptional(e => e.Administrator)
                .HasForeignKey(e => e.CreatedBy);

            modelBuilder.Entity<Administrator>()
                .HasMany(e => e.Videos1)
                .WithOptional(e => e.Administrator1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<Business>()
                .Property(e => e.BusinessId)
                .IsUnicode(false);

            modelBuilder.Entity<CatNew>()
                .HasMany(e => e.ContenNews)
                .WithOptional(e => e.CatNew)
                .HasForeignKey(e => e.CategoryID);

            modelBuilder.Entity<CatVideo>()
                .HasMany(e => e.Videos)
                .WithOptional(e => e.CatVideo)
                .HasForeignKey(e => e.CategoryID);

            modelBuilder.Entity<Permission>()
                .Property(e => e.PermissionName)
                .IsUnicode(false);

            modelBuilder.Entity<Permission>()
                .Property(e => e.BusinessId)
                .IsUnicode(false);

            modelBuilder.Entity<Permission>()
                .HasMany(e => e.GrantPermissions)
                .WithRequired(e => e.Permission)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Video>()
                .Property(e => e.Keyoutube)
                .IsUnicode(false);
        }
    }
}
