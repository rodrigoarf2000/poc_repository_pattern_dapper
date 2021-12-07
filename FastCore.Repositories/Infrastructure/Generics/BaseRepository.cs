using Dapper;
using FastCore.Repositories.Infrastructure.DbFactory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FastCore.Repositories.Infrastructure.Generics
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly IDatabaseFactory _databaseFactory;
        private readonly IDbConnection _connection;

        public BaseRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
            _connection = _databaseFactory.CreateConnection();
        }

        /// <summary>
        /// Adiciona um objeto ao banco de dados e retorna o número de linhas afetadas.
        /// </summary>
        public async Task<int> AddAsync(TEntity entity)
        {
            try
            {
                var columns = GetColumns();
                var columnKey = $"{typeof(TEntity).Name}Id";
                var columnConcat = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
                var sql = $@"INSERT INTO {typeof(TEntity).Name} ({columns}) OUTPUT INSERTED.{columnKey} VALUES ({columnConcat})";

                using (_connection)
                {
                    var result = await _connection.ExecuteAsync(sql, entity);
                    return await Task.FromResult(result);
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

        /// <summary>
        /// Adiciona um objeto ao banco de dados e retorna ele novamente após adicionado.
        /// </summary>
        public async Task<TEntity> AddAndGetItemAsync(TEntity entity)
        {
            try
            {
                var columns = GetColumns();
                var columnKey = $"{typeof(TEntity).Name}Id";
                var columnConcat = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
                var sql = $@"INSERT INTO {typeof(TEntity).Name} ({columns}) OUTPUT INSERTED.{columnKey} VALUES ({columnConcat})";

                using (_connection)
                {
                    var id = _connection.QuerySingle<int>(sql, entity);
                    var result = await GetByIdAsync(id);
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

        /// <summary>
        /// Atualiza um objeto existente no banco de dados.
        /// </summary>
        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            try
            {
                var columns = GetColumns();
                var columnKey = $"{typeof(TEntity).Name}Id";
                var columnConcat = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
                var sql = $"UPDATE {typeof(TEntity).Name} SET {columnConcat} WHERE {columnKey}=@{columnKey}";

                using (_connection)
                {
                    var result = await _connection.ExecuteAsync(sql, entity);
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

        /// <summary>
        /// Atualiza um objeto existente no banco de dados e retorna ele atualizado.
        /// </summary>
        public async Task<TEntity> UpdateAndGetItemAsync(TEntity entity)
        {
            try
            {
                var columns = GetColumns();
                var columnKey = $"{typeof(TEntity).Name}Id";
                var columnConcat = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
                var sql = $"UPDATE {typeof(TEntity).Name} SET {columnConcat} OUTPUT INSERTED.{columnKey} WHERE {columnKey}=@{columnKey}";

                using (_connection)
                {
                    var id = _connection.QuerySingle<int>(sql, entity);
                    var result = await GetByIdAsync(id);
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

        /// <summary>
        /// Obtém um objeto pelo seu id se existir no banco de dados.
        /// </summary>
        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            try
            {
                var columns = GetColumns();
                var columnKey = $"{typeof(TEntity).Name}Id";
                var columnConcat = string.Join(", ", columns);
                var sql = $"SELECT {columnKey}, {columnConcat} FROM {typeof(TEntity).Name} WHERE {columnKey}=@Id";

                using (_connection)
                {
                    var result = await _connection.QuerySingleOrDefaultAsync<TEntity>(sql, new { Id = id });
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

        /// <summary>
        /// Obtém todos os objetos cadastrados no banco de dados.
        /// </summary>
        public async Task<List<TEntity>> GetAllAsync()
        {
            try
            {
                var columns = GetColumns();
                var columnKey = $"{typeof(TEntity).Name}Id";
                var columnConcat = string.Join(", ", columns);
                var sql = $"SELECT {columnKey}, {columnConcat} FROM {typeof(TEntity).Name}";

                using (_connection)
                {
                    var result = await _connection.QueryAsync<TEntity>(sql);
                    return result.ToList();
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

        /// <summary>
        /// Apaga um objeto do banco de dados pelo seu id e retorna o número de linhas afetadas.
        /// </summary>
        public async Task<int> DeleteByIdAsync(int id)
        {
            try
            {
                var columnKey = $"{typeof(TEntity).Name}Id";
                var sql = $"DELETE FROM {typeof(TEntity).Name} WHERE {columnKey}=@Id";

                using (_connection)
                {
                    var result = await _connection.ExecuteAsync(sql, new { Id = id });
                    return await Task.FromResult(result);
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

        /// <summary>
        /// Obtém as propriedades do modelo, que são as colunas da tabela do banco de dados a qual o modelo se refere.
        /// </summary>
        public List<string> GetColumns()
        {
            try
            {
                var columnKey = $"{typeof(TEntity).Name}Id";
                var collection = typeof(TEntity).GetProperties().Where(e => e.Name != columnKey).Select(e => e.Name).ToList();
                return collection;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
