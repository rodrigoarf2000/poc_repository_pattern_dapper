using Dapper;
using FastCore.Repositories.Entities;
using FastCore.Repositories.Infrastructure.DbFactory;
using FastCore.Repositories.Infrastructure.Generics;
using System;
using System.Data;
using System.Threading.Tasks;

namespace FastCore.Repositories
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        /// <summary>
        /// Obtem um livro pelo código Isbn.
        /// </summary>
        Task<Book> GetItemByIsbnAsync(string isbn);
    }

    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly IDatabaseFactory _databaseFactory;
        private readonly IDbConnection _connection;

        public BookRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            _databaseFactory = databaseFactory;
            _connection = _databaseFactory.CreateConnection();
        }

        /// <summary>
        /// Obtem um livro pelo código Isbn.
        /// </summary>
        public async Task<Book> GetItemByIsbnAsync(string isbn)
        {
            try
            {
                var columns = GetColumns();
                var columnKey = $"{typeof(Book).Name}Id";
                var columnConcat = string.Join(", ", columns);
                var sql = $"SELECT {columnKey}, {columnConcat} FROM {typeof(Book).Name} WHERE {columnKey}=@Id";

                using (_connection)
                {
                    var result = await _connection.QuerySingleOrDefaultAsync<Book>(sql, new { Isbn = isbn });
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }
    }
}
