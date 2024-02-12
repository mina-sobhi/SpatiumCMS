namespace Domain.BlogsAggregate
{
    public interface IBlogRepository
    {
        Task<IEnumerable<Blog>> FindAllAsync(int lastPageId, int NumberofRecord);

         Task<Blog> GetByIdAsync(int id);
        Task<IEnumerable<Blog>> GetBlogsAsync();
        Task CreateAsync(Blog blog);
        Task UpdateAsync(Blog blog);
        Task DeleteAsync(int id);

    }
}
