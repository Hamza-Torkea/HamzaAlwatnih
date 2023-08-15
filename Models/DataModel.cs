using alwatnia.Migrations;
using alwatnia.Models.JobCV;
using System.Data.Entity;
using JobApplication = alwatnia.Models.JobCV.JobApplication;

namespace alwatnia.Models
{
    public class DataModel : DbContext
    {
        public DataModel()
            : base("name=DataModel")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }

        public virtual DbSet<Activity> Activities { get; set; }

        public virtual DbSet<Advertise> Advertises { get; set; }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<Company> Companies { get; set; }

        public virtual DbSet<Configration> Configrations { get; set; }

        public virtual DbSet<Contact> Contacts { get; set; }

        public virtual DbSet<Image> Images { get; set; }

        public virtual DbSet<Job> Jobs { get; set; }

        public virtual DbSet<Medium> Media { get; set; }

        public virtual DbSet<News> News { get; set; }

        public virtual DbSet<Page> Pages { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Slider> Sliders { get; set; }

        public virtual DbSet<Staff> Staffs { get; set; }

        public virtual DbSet<Exctive> Exctives { get; set; }
        public virtual DbSet<Reports> Report { get; set; }

        public virtual DbSet<Album> Albums { get; set; }

        public virtual DbSet<UsersProducts> UsersProducts { get; set; }

        public virtual DbSet<RequestedProducts> RequestedProduct { get; set; }

        public virtual DbSet<MailingList> MailingLists { get; set; }

        public virtual DbSet<MailCats> MailCat { get; set; }

        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        public virtual DbSet<ProductCat> ProductCats { get; set; }

        public virtual DbSet<JobApplication> JobApplication { get; set; }

        public virtual DbSet<Qualification> Qualifications { get; set; }

        public virtual DbSet<LanguageSkill> LanguageSkills { get; set; }

        public virtual DbSet<ComputerSkill> ComputerSkills { get; set; }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Employment> Employments { get; set; }

        public virtual DbSet<RelativeEmployer> RelativeEmployers { get; set; }

        public virtual DbSet<Reference> References { get; set; }

        public virtual DbSet<NewsCompany> NewsCompanies { get; set; }

        public virtual DbSet<Branch> Branches { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataModel, Configuration>());

            modelBuilder.Entity<AspNetRole>().HasMany(e => e.AspNetUsers).WithMany(e => e.AspNetRoles).Map(m =>
                m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));


            modelBuilder.Entity<AspNetUser>().HasMany(e => e.AspNetUserClaims).WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);


            modelBuilder.Entity<AspNetUser>().HasMany(e => e.AspNetUserLogins).WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Company>().Property(e => e.Map).IsRequired();
            //            modelBuilder.Entity<Company>().Property(e => e.lat).HasPrecision(12, 7);
            //
            //            modelBuilder.Entity<Company>().Property(e => e.lng).HasPrecision(12, 7);

            modelBuilder.Entity<Product>().Property(e => e.Cost).HasPrecision(12, 3);

            modelBuilder.Entity<Product>().Property(e => e.Discount_Cost).HasPrecision(12, 3);

            modelBuilder.Entity<Product>().HasMany(e => e.Comments).WithOptional(e => e.Product)
                .HasForeignKey(e => e.product_id);

            modelBuilder.Entity<News>().HasMany(e => e.Comments).WithOptional(e => e.News)
                .HasForeignKey(e => e.news_id);

            modelBuilder.Entity<Activity>().HasMany(e => e.Comments).WithOptional(e => e.Activity)
                .HasForeignKey(e => e.act_id);

            modelBuilder.Entity<Job>().HasMany(e => e.Comments).WithOptional(e => e.Job).HasForeignKey(e => e.job_id);

            modelBuilder.Entity<Company>().HasMany(e => e.Page).WithOptional(e => e.Company)
                .HasForeignKey(e => e.company_id);

            modelBuilder.Entity<Company>().HasMany(e => e.Reports).WithOptional(e => e.Company)
                .HasForeignKey(e => e.company_id);

            modelBuilder.Entity<Company>().HasMany(e => e.Staff).WithOptional(e => e.Company)
                .HasForeignKey(e => e.company_id);

            modelBuilder.Entity<Album>().HasMany(e => e.Image).WithOptional(e => e.Album).HasForeignKey(e => e.AlbumId);

            modelBuilder.Entity<Company>().HasMany(e => e.Product).WithOptional(e => e.Company)
                .HasForeignKey(e => e.company_Id);

            modelBuilder.Entity<Company>().HasMany(e => e.Image).WithOptional(e => e.Company)
                .HasForeignKey(e => e.companyId);

            modelBuilder.Entity<Page>().HasMany(e => e.Image).WithOptional(e => e.Page)
                .HasForeignKey(e => e.PageId);

            modelBuilder.Entity<Company>().HasMany(e => e.UsersProducts).WithOptional(e => e.Company)
                .HasForeignKey(e => e.company_id);

            modelBuilder.Entity<Company>().HasMany(e => e.RequestedProduct).WithOptional(e => e.Company)
                .HasForeignKey(e => e.company_id);

            modelBuilder.Entity<Product>().HasMany(e => e.RequestedProduct).WithOptional(e => e.Product)
                .HasForeignKey(e => e.product_id);

            modelBuilder.Entity<ProductCat>().HasMany(e => e.Products).WithOptional(e => e.ProductCat)
                .HasForeignKey(e => e.ProductCatId);

            modelBuilder.Entity<Company>().HasMany(e => e.ProductCats).WithOptional(e => e.Company)
                .HasForeignKey(e => e.CompanyId);

            modelBuilder.Entity<UsersProducts>().HasMany(e => e.RequestedProduct).WithOptional(e => e.UsersProduct)
                .HasForeignKey(e => e.UserProductId);

            modelBuilder.Entity<Qualification>()
                .HasRequired(e => e.JobApplication)
                .WithMany(e => e.Qualifications)
                .HasForeignKey(t => t.JobApplicationId);

            modelBuilder.Entity<LanguageSkill>()
                .HasRequired(e => e.JobApplication)
                .WithMany(e => e.LanguageSkills)
                .HasForeignKey(t => t.JobApplicationId);

            modelBuilder.Entity<ComputerSkill>()
                .HasRequired(e => e.JobApplication)
                .WithMany(e => e.ComputerSkills)
                .HasForeignKey(t => t.JobApplicationId);

            modelBuilder.Entity<Course>()
                .HasRequired(e => e.JobApplication)
                .WithMany(e => e.Courses)
                .HasForeignKey(t => t.JobApplicationId);

            modelBuilder.Entity<Employment>()
                .HasRequired(e => e.JobApplication)
                .WithMany(e => e.Employments)
                .HasForeignKey(t => t.JobApplicationId);

            modelBuilder.Entity<RelativeEmployer>()
                .HasRequired(e => e.JobApplication)
                .WithMany(e => e.RelativeEmployers)
                .HasForeignKey(t => t.JobApplicationId);

            modelBuilder.Entity<Reference>()
                .HasRequired(e => e.JobApplication)
                .WithMany(e => e.References)
                .HasForeignKey(t => t.JobApplicationId);

            modelBuilder.Entity<Contact>()
                .HasOptional(e => e.Company)
                .WithMany(e => e.Contacts)
                .HasForeignKey(t => t.CompanyId);
        }
    }
}