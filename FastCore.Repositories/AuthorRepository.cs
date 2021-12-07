using FastCore.Repositories.Entities;
using FastCore.Repositories.Infrastructure.DbFactory;
using FastCore.Repositories.Infrastructure.Generics;

namespace FastCore.Repositories
{
    public interface IAuthorRepository : IBaseRepository<Author>
    {
    }

    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        private readonly IDatabaseFactory _databaseFactory;

        public AuthorRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
    }
}
