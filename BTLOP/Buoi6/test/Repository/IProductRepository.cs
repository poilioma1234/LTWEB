using test.Models;

namespace test.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);

        Task<IEnumerable<Product>> GetByOwnerAsync(string ownerId);

        Task<Product?> GetByIdAsync(int id);

        Task<Product?> GetByIdForOwnerAsync(int id, string ownerId);

        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(int id);
    }
}
