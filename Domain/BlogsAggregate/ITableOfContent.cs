using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BlogsAggregate
{
    public interface ITableOfContent
    {
        Task<TableOfContent> GetByIdAsync(int id);
        Task<IEnumerable<TableOfContent>> GetBlogsAsync();
        Task CreateAsync(TableOfContent tableOfContent);
        Task UpdateAsync(TableOfContent tableOfContent);
        Task DeleteAsync(int id);
    }
}
