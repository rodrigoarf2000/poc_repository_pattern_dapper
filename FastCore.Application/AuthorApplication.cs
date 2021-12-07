using FastCore.Application.Commom;
using FastCore.Application.Entities;
using FastCore.Repositories;
using FastCore.Repositories.Entities;
using FastCore.Repositories.Infrastructure.Interfaces.Cache;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastCore.Application
{
    public interface IAuthorApplication
    {
        Task AddAsync(AuthorVm model);
        Task UpdateAsync(AuthorVm model);
        Task DeleteByIdAsync(AuthorVm model);
        Task<AuthorVm> GetByIdAsync(int id);
        Task<List<AuthorVm>> GetAllAsync();
    }

    public class AuthorApplication : IAuthorApplication
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheRepository _cacheRepository;
        private readonly IAuthorRepository _authorRepository;

        public AuthorApplication(IConfiguration configuration, ICacheRepository cacheRepository, IAuthorRepository bookRepository)
        {
            _configuration = configuration;
            _authorRepository = bookRepository;
            _cacheRepository = cacheRepository;
        }

        public async Task AddAsync(AuthorVm model)
        {
            var entity = model.ToModelView<Author, AuthorVm>();
            await _authorRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(AuthorVm model)
        {
            var entity = model.ToModelView<Author, AuthorVm>();
            await _authorRepository.UpdateAsync(entity);
        }

        public async Task DeleteByIdAsync(AuthorVm entity)
        {
            await _authorRepository.DeleteByIdAsync(entity.AuthorId);   
        }

        public async Task<AuthorVm> GetByIdAsync(int id)
        {
            var entity = await _authorRepository.GetByIdAsync(id);
            var result = entity.ToViewModel<Author, AuthorVm>();
            return result;
        }

        public async Task<List<AuthorVm>> GetAllAsync()
        {
            var collection = await _authorRepository.GetAllAsync();
            var result = collection.ToViewModel<List<Author>, List<AuthorVm>>();
            return result;
        }
    }
}
