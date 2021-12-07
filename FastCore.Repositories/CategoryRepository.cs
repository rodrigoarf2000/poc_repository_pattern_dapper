using FastCore.Repositories.Entities;
using FastCore.Repositories.Infrastructure.DbFactory;
using FastCore.Repositories.Infrastructure.Generics;

namespace FastCore.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
    }

    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly IDatabaseFactory _databaseFactory;

        public CategoryRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
    }
}
