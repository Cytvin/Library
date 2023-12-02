using LibraryDAL.EFModels;

namespace LibraryDAL
{
    public class BookFacade : IDataBaseFacade<Book>
    {
        private readonly LibraryDbContext _context;

        public BookFacade(LibraryDbContext context)
        {
            _context = context;
        }

        public void Delete(Book entity)
        {
            _context.Books.Remove(entity);
            _context.SaveChanges();
        }

        public Book? GetById(int id)
        {
            return _context.Books.Find(id);
        }

        public void Insert(Book entity)
        {
            _context.Books.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Book> SelectAll()
        {
            return _context.Books.ToList();
        }

        public void Update(Book entity)
        {
            Book? book = GetById(entity.Id);

            if (book == null)
            {
                throw new ArgumentOutOfRangeException(nameof(entity.Id), $"Не найдена книга с Id={entity.Id}");
            }

            book.Name = entity.Name;
            book.Author = entity.Author;
            book.PublishingYear = entity.PublishingYear;

            _context.Books.Update(book);
            _context.SaveChanges();
        }
    }
}
