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
    public interface ICategoryApplication
    {
        Task AddAsync(CategoryVm model);
        Task UpdateAsync(CategoryVm model);
        Task DeleteByIdAsync(CategoryVm model);
        Task<CategoryVm> GetByIdAsync(int categoryId);
        Task<List<CategoryVm>> GetAllAsync();
    }

    public class CategoryApplication : ICategoryApplication
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheRepository _cacheRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryApplication(IConfiguration configuration, ICacheRepository cacheRepository, ICategoryRepository categoryRepository)
        {
            _configuration = configuration;
            _categoryRepository = categoryRepository;
            _cacheRepository = cacheRepository;
        }

        public async Task AddAsync(CategoryVm model)
        {
            var entity = model.ToModelView<Category, CategoryVm>();
            await _categoryRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(CategoryVm model)
        {
            var entity = model.ToModelView<Category, CategoryVm>();
            await _categoryRepository.UpdateAsync(entity);
        }

        public async Task DeleteByIdAsync(CategoryVm entity)
        {
            await _categoryRepository.DeleteByIdAsync(entity.CategoryId);         
        }

        public async Task<CategoryVm> GetByIdAsync(int categoryId)
        {
            var entity = await _categoryRepository.GetByIdAsync(categoryId);
            var result = entity.ToViewModel<Category, CategoryVm>();
            return result;
        }

        public async Task<List<CategoryVm>> GetAllAsync()
        {
            var collection = await _categoryRepository.GetAllAsync();
            var result = collection.ToViewModel<List<Category>, List<CategoryVm>>();
            return result;
        }
    }
}
