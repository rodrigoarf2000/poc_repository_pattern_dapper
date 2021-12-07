using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastCore.Repositories.Infrastructure.Generics
{
    public interface IBaseRepository<TEntity>
    {
        /// <summary>
        /// Adiciona um objeto ao banco de dados e retorna o número de linhas afetadas.
        /// </summary>
        Task<int> AddAsync(TEntity entity);
        /// <summary>
        /// Adiciona um objeto ao banco de dados e retorna ele novamente após adicionado.
        /// </summary>
        Task<TEntity> AddAndGetItemAsync(TEntity entity);
        /// <summary>
        /// Atualiza um objeto existente no banco de dados.
        /// </summary>
        Task<int> UpdateAsync(TEntity entity);
        /// <summary>
        /// Atualiza um objeto existente no banco de dados e retorna ele atualizado.
        /// </summary>
        Task<TEntity> UpdateAndGetItemAsync(TEntity entity);
        /// <summary>
        /// Obtém um objeto pelo seu id se existir no banco de dados.
        /// </summary>
        Task<TEntity> GetByIdAsync(int id);
        /// <summary>
        /// Obtém todos os objetos cadastrados no banco de dados.
        /// </summary>
        Task<List<TEntity>> GetAllAsync();
        /// <summary>
        /// Apaga um objeto do banco de dados pelo seu id e retorna o número de linhas afetadas.
        /// </summary>
        Task<int> DeleteByIdAsync(int id);
    }
}
