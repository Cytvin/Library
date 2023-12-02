using LibraryDAL.EFModels;

namespace LibraryDAL
{
    public class PersonFacade : IDataBaseFacade<Person>
    {
        private readonly LibraryDbContext _context;

        public PersonFacade(LibraryDbContext context)
        {
            _context = context;
        }

        public void Delete(Person entity)
        {
            _context.People.Remove(entity);
            _context.SaveChanges();
        }

        public Person? GetById(int id)
        {
            return _context.People.Find(id);
        }

        public void Insert(Person entity)
        {
            _context.People.Add(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Person> SelectAll()
        {
            return _context.People.ToList();
        }

        public void Update(Person entity)
        {
            Person? person = GetById(entity.Id);

            if (person == null)
            {
                throw new ArgumentOutOfRangeException(nameof(entity.Id), $"Не найден пользователь с Id={entity.Id}");
            }

            person.Fio = entity.Fio;

            _context.People.Update(person);
            _context.SaveChanges();
        }
    }
}
