using LibraryDAL.EFModels;

namespace LibraryDAL
{
    public class BookReservationFacade : IDataBaseFacade<BookReservation>
    {
        private readonly LibraryDbContext _context;

        public BookReservationFacade(LibraryDbContext context) 
        {  
            _context = context; 
        }

        public void Delete(BookReservation entity)
        {
            _context.BookReservations.Remove(entity);
            _context.SaveChanges();
        }

        public BookReservation? GetById(int id)
        {
            return _context.BookReservations.Find(id);
        }

        public void Insert(BookReservation entity)
        {
            _context.BookReservations.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<BookReservation> SelectAll()
        {
            return _context.BookReservations.ToList();
        }

        public void Update(BookReservation entity)
        {
            BookReservation? bookReservation = GetById(entity.Id);

            if (bookReservation == null)
            {
                throw new ArgumentOutOfRangeException(nameof(entity.Id), $"Не найдена запись резервирования книги с Id={entity.Id}");
            }

            bookReservation.PeopleId = entity.PeopleId;
            bookReservation.BookId = entity.BookId;
            bookReservation.Date = entity.Date;

            _context.Update(bookReservation);
            _context.SaveChanges();
        }
    }
}
