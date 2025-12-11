using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Doan.Models;

public partial class DoanContext : DbContext
{
    public DoanContext()
    {
    }

    public DoanContext(DbContextOptions<DoanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbAbout> TbAbouts { get; set; }

    public virtual DbSet<TbAboutDetail> TbAboutDetails { get; set; }

    public virtual DbSet<TbAccount> TbAccounts { get; set; }

    public virtual DbSet<TbBlog> TbBlogs { get; set; }

    public virtual DbSet<TbCategory> TbCategories { get; set; }

    public virtual DbSet<TbContact> TbContacts { get; set; }

    public virtual DbSet<TbDepartment> TbDepartments { get; set; }

    public virtual DbSet<TbDoctor> TbDoctors { get; set; }

    public virtual DbSet<TbMenu> TbMenus { get; set; }

    public virtual DbSet<TbNews> TbNews { get; set; }

    public virtual DbSet<TbNewsSimple> TbNewsSimples { get; set; }

    public virtual DbSet<TbProduct> TbProducts { get; set; }

    public virtual DbSet<TbProductCategory> TbProductCategories { get; set; }

    public virtual DbSet<TbProductReview> TbProductReviews { get; set; }

    public virtual DbSet<TbRole> TbRoles { get; set; }

    public virtual DbSet<TbService> TbServices { get; set; }

    public virtual DbSet<TbServiceBooking> TbServiceBookings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=ADMIN-PC; initial catalog=Doan; integrated security=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbAbout>(entity =>
        {
            entity.HasKey(e => e.AboutId).HasName("PK__tb_About__717FC93CC33CF5A2");

            entity.ToTable("tb_About");

            entity.Property(e => e.Alias).HasMaxLength(250);
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.SeoDescription).HasMaxLength(500);
            entity.Property(e => e.SeoKeywords).HasMaxLength(250);
            entity.Property(e => e.SeoTitle).HasMaxLength(250);
            entity.Property(e => e.ShortDescription).HasMaxLength(1000);
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Category).WithMany(p => p.TbAbouts)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_tb_About_tb_Category");
        });

        modelBuilder.Entity<TbAboutDetail>(entity =>
        {
            entity.HasKey(e => e.AboutDetailsId).HasName("PK__tb_About__421546584C389B2D");

            entity.ToTable("tb_AboutDetails");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.ContactFormText).HasMaxLength(1000);
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MapLink).HasMaxLength(500);
            entity.Property(e => e.ModifiedBy).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.SocialLinks).HasMaxLength(1000);

            entity.HasOne(d => d.About).WithMany(p => p.TbAboutDetails)
                .HasForeignKey(d => d.AboutId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_AboutD__About__6FE99F9F");
        });

        modelBuilder.Entity<TbAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__tb_Accou__349DA5A64F0FDEFF");

            entity.ToTable("tb_Account");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.TbAccounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__tb_Accoun__RoleI__3A81B327");
        });

        modelBuilder.Entity<TbBlog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__tb_Blog__54379E3084E8F609");

            entity.ToTable("tb_Blog");

            entity.Property(e => e.Alias).HasMaxLength(250);
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.SeoDescription).HasMaxLength(500);
            entity.Property(e => e.SeoKeywords).HasMaxLength(250);
            entity.Property(e => e.SeoTitle).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Account).WithMany(p => p.TbBlogs)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__tb_Blog__Account__5AEE82B9");

            entity.HasOne(d => d.Category).WithMany(p => p.TbBlogs)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__tb_Blog__Categor__59FA5E80");
        });

        modelBuilder.Entity<TbCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__tb_Categ__19093A0B9CAFEA16");

            entity.ToTable("tb_Category");

            entity.Property(e => e.Alias).HasMaxLength(150);
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ModifiedBy).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.SeoDescription).HasMaxLength(500);
            entity.Property(e => e.SeoKeywords).HasMaxLength(250);
            entity.Property(e => e.SeoTitle).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(150);
        });

        modelBuilder.Entity<TbContact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__tb_Conta__5C66259BC24980F5");

            entity.ToTable("tb_Contact");

            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.ModifiedBy).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Phone).HasMaxLength(50);
        });

        modelBuilder.Entity<TbDepartment>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__tb_Depar__B2079BED6DA777D6");

            entity.ToTable("tb_Departments");

            entity.Property(e => e.Alias).HasMaxLength(150);
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DepartmentName).HasMaxLength(150);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.SeoDescription).HasMaxLength(500);
            entity.Property(e => e.SeoKeywords).HasMaxLength(250);
            entity.Property(e => e.SeoTitle).HasMaxLength(250);

            entity.HasOne(d => d.Category).WithMany(p => p.TbDepartments)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__tb_Depart__Categ__68487DD7");

            entity.HasOne(d => d.Doctor).WithMany(p => p.TbDepartments)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__tb_Depart__Docto__693CA210");
        });

        modelBuilder.Entity<TbDoctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__tb_Docto__2DC00EBF99D3A6D0");

            entity.ToTable("tb_Doctor");

            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Position).HasMaxLength(100);
            entity.Property(e => e.Qualification).HasMaxLength(200);

            entity.HasOne(d => d.Account).WithMany(p => p.TbDoctors)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__tb_Doctor__Accou__412EB0B6");

            entity.HasOne(d => d.Category).WithMany(p => p.TbDoctors)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__tb_Doctor__Categ__403A8C7D");
        });

        modelBuilder.Entity<TbMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__tb_Menu__C99ED230E1B293E0");

            entity.ToTable("tb_Menu");

            entity.Property(e => e.Alias).HasMaxLength(150);
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(150);
        });

        modelBuilder.Entity<TbNews>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__tb_News__954EBDF3880A05D0");

            entity.ToTable("tb_News");

            entity.Property(e => e.Alias).HasMaxLength(250);
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.SeoDescription).HasMaxLength(500);
            entity.Property(e => e.SeoKeywords).HasMaxLength(250);
            entity.Property(e => e.SeoTitle).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Category).WithMany(p => p.TbNews)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__tb_News__Categor__5EBF139D");
        });

        modelBuilder.Entity<TbNewsSimple>(entity =>
        {
            entity.HasKey(e => e.NewsSimpleId).HasName("PK__tb_NewsS__D7592A5F1D51E6B4");

            entity.ToTable("tb_NewsSimple");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        modelBuilder.Entity<TbProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__tb_Produ__B40CC6CD19512A4A");

            entity.ToTable("tb_Product");

            entity.Property(e => e.Alias).HasMaxLength(250);
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Duration).HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Doctor).WithMany(p => p.TbProducts)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__tb_Produc__Docto__5165187F");

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.TbProducts)
                .HasForeignKey(d => d.ProductCategoryId)
                .HasConstraintName("FK__tb_Produc__Produ__5070F446");
        });

        modelBuilder.Entity<TbProductCategory>(entity =>
        {
            entity.HasKey(e => e.ProductCategoryId).HasName("PK__tb_Produ__3224ECCE40A7A22D");

            entity.ToTable("tb_ProductCategory");

            entity.Property(e => e.Alias).HasMaxLength(150);
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Icon).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(150);
        });

        modelBuilder.Entity<TbProductReview>(entity =>
        {
            entity.HasKey(e => e.ProductReviewId).HasName("PK__tb_Produ__396318809D97BB0B");

            entity.ToTable("tb_ProductReview");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Detail).HasMaxLength(500);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);

            entity.HasOne(d => d.Doctor).WithMany(p => p.TbProductReviews)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__tb_Produc__Docto__5629CD9C");

            entity.HasOne(d => d.Product).WithMany(p => p.TbProductReviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__tb_Produc__Produ__5535A963");
        });

        modelBuilder.Entity<TbRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__tb_Role__8AFACE1AB1625205");

            entity.ToTable("tb_Role");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<TbService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__tb_Servi__C51BB00AFEC7ACF2");

            entity.ToTable("tb_Service");

            entity.Property(e => e.Alias).HasMaxLength(250);
            entity.Property(e => e.CreatedBy).HasMaxLength(150);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Icon).HasMaxLength(500);
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(150);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.SeoDescription).HasMaxLength(500);
            entity.Property(e => e.SeoKeywords).HasMaxLength(250);
            entity.Property(e => e.SeoTitle).HasMaxLength(250);
            entity.Property(e => e.ShortDescription).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Category).WithMany(p => p.TbServices)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__tb_Servic__Categ__47DBAE45");

            entity.HasOne(d => d.Doctor).WithMany(p => p.TbServices)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__tb_Servic__Docto__46E78A0C");
        });

        modelBuilder.Entity<TbServiceBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__tb_Servi__73951AED5B7A2A80");

            entity.ToTable("tb_ServiceBooking");

            entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.PatientName).HasMaxLength(150);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Service).WithMany(p => p.TbServiceBookings)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Servic__Servi__51300E55");
        });

        modelBuilder.Entity<TbServiceBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__tb_Servi__73951AED5B7A2A80");

            entity.ToTable("tb_ServiceBooking");

            entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.PatientName).HasMaxLength(150);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Service).WithMany(p => p.TbServiceBookings)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Servic__Servi__51300E55");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
