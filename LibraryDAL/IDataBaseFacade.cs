namespace LibraryDAL
{
    public interface IDataBaseFacade<T>
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> SelectAll();
        T? GetById(int id);
    }
}
