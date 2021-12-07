using FastCore.Application.Commom;
using FastCore.Application.Entities;
using FastCore.Repositories;
using FastCore.Repositories.Entities;
using FastCore.Repositories.Infrastructure.Cache;
using FastCore.Repositories.Infrastructure.Interfaces.Cache;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastCore.Application
{
    public interface IBookApplication
    {
        Task AddAsync(BookVm model);
        Task UpdateAsync(BookVm model);
        Task DeleteByIdAsync(BookVm model);
        Task<BookVm> GetByIdAsync(int id);
        Task<List<BookVm>> GetAllAsync();
        Task<BookVm> GetItemByIsbnAsync(string isbn);
    }

    public class BookApplication : IBookApplication
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheRepository _cacheRepository;
        private readonly IBookRepository _bookRepository;

        public BookApplication(IConfiguration configuration, ICacheRepository cacheRepository, IBookRepository bookRepository)
        {
            _configuration = configuration;
            _bookRepository = bookRepository;
            _cacheRepository = cacheRepository;
        }

        public async Task AddAsync(BookVm model)
        {
            var entity = model.ToModelView<Book, BookVm>();
            var itemAdded = await _bookRepository.AddAndGetItemAsync(entity);

            // Após adicionar o item no banco, adiciona no cache.
            model = itemAdded.ToViewModel<Book, BookVm>();
            await _cacheRepository.AddAsync($"{CacheKeyType.KEY_BOOK_ID}_{model.BookId}", JsonConvert.SerializeObject(model));
        }

        public async Task UpdateAsync(BookVm model)
        {
            var entity = model.ToModelView<Book, BookVm>();
            var itemUpdated = await _bookRepository.UpdateAndGetItemAsync(entity);
            model = itemUpdated.ToViewModel<Book, BookVm>();

            // Após atualizar o item no banco, atualiza no cache.
            await _cacheRepository.UpdateAsync($"{CacheKeyType.KEY_BOOK_ID}_{model.BookId}", JsonConvert.SerializeObject(model));
        }

        public async Task DeleteByIdAsync(BookVm model)
        {
            await _bookRepository.DeleteByIdAsync(model.BookId);
            var itemDeleted = await _bookRepository.GetByIdAsync(model.BookId);

            // Após remover o item no banco, remove no cache.
            if (itemDeleted == null) { await _cacheRepository.RemoveAsync($"{CacheKeyType.KEY_BOOK_ID}_{model.BookId}"); }            
        }

        public async Task<BookVm> GetByIdAsync(int id)
        {
            // Tenta obter o item primeiro do cache se não tiver obtem do banco.
            var bookFromCache = await _cacheRepository.GetAsync($"{CacheKeyType.KEY_BOOK_ID}_{id}");

            if (!string.IsNullOrEmpty(bookFromCache))
            {
                var result = JsonConvert.DeserializeObject<BookVm>(bookFromCache);
                return result;
            }
            else
            {
                var entity = await _bookRepository.GetByIdAsync(id);
                var result = entity.ToViewModel<Book, BookVm>();
                return result;
            }
        }

        public async Task<BookVm> GetItemByIsbnAsync(string isbn)
        {
            // Obtem o item direto do banco quando a busca é por Isbn, nao utilizando cache.
            var entity = await _bookRepository.GetItemByIsbnAsync(isbn);
            var result = entity.ToViewModel<Book, BookVm>();
            return result;
        }

        public async Task<List<BookVm>> GetAllAsync()
        {
            var collection = await _bookRepository.GetAllAsync();
            var result = collection.ToViewModel<List<Book>, List<BookVm>>();
            return result;
        }
    }
}
