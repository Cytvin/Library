using Microsoft.EntityFrameworkCore;

namespace LibraryDAL.EFModels;

public partial class LibraryDbContext : DbContext
{
    public LibraryDbContext()
    {
    }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookReservation> BookReservations { get; set; }

    public virtual DbSet<Person> People { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseLazyLoadingProxies()
        .UseSqlServer("Server=.\\SQLExpress;Database=LibraryDB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(e => e.Author)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsFixedLength();
        });

        modelBuilder.Entity<BookReservation>(entity =>
        {
            entity.ToTable("BookReservation");

            entity.HasOne(d => d.Book).WithMany(p => p.BookReservations)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_BookReservation_Books");

            entity.HasOne(d => d.People).WithMany(p => p.BookReservations)
                .HasForeignKey(d => d.PeopleId)
                .HasConstraintName("FK_BookReservation_People");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.Property(e => e.Fio)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
