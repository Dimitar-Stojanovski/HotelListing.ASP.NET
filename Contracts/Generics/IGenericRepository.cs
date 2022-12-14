using HotelListing.API.Data.Models;


namespace HotelListing.API.Contracts
{
    public interface IGenericRepository<T>where T : class
    {
        Task<T> GetAsync(int? id); //? is nullable
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);
        Task<bool> Exist(int id);

        //Implementing Page Result
        Task<PageResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
    }
}
