//using Domain.BlogsAggregate;
//using Infrastructure.Database.Database;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Database.Repository
//{
//    internal class TableOfContentRepository : RepositoryBase, ITableOfContent
//    {
//        public TableOfContentRepository(SpatiumDbContent SpatiumDbContent) : base(SpatiumDbContent)
//        {
//        }
//        public async Task CreateAsync(TableOfContent tableOfContent)
//        {
//           await SpatiumDbContent.TableOfContents.AddAsync(tableOfContent);
//        }

//        public async Task DeleteAsync(int id)
//        {
//            SpatiumDbContent.TableOfContents.Remove(await GetByIdAsync(id));
//        }

//        public async Task<IEnumerable<TableOfContent>> GetBlogsAsync()
//        {
//            return await SpatiumDbContent.TableOfContents.ToListAsync();
//        }

//        public async Task<TableOfContent> GetByIdAsync(int id)
//        {
//            return await SpatiumDbContent.TableOfContents.FindAsync(id);
//        }

//        public void UpdateAsync(TableOfContent tableOfContent)
//        {
//            SpatiumDbContent.TableOfContents.Update(tableOfContent);
//        }
//    }
//}
