using FastCore.Application;
using FastCore.Application.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FastCore.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryApplication _categoryApplication;

        public CategoryController(ILogger<CategoryController> logger, ICategoryApplication categoryApplication)
        {
            _logger = logger;
            _categoryApplication = categoryApplication;
        }

        /// <summary>
        /// Adiciona uma categoria nova.
        /// </summary>
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromBody] CategoryVm entity)
        {
            await _categoryApplication.AddAsync(entity);
            return Ok();
        }

        /// <summary>
        /// Atualiza uma categoria existente.
        /// </summary>
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] CategoryVm entity)
        {
            await _categoryApplication.UpdateAsync(entity);
            return Ok();
        }

        /// <summary>
        /// Apaga uma categoria existente.
        /// </summary>
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteByIdAsync([FromBody] CategoryVm entity)
        {
            await _categoryApplication.DeleteByIdAsync(entity);
            return Ok();
        }

        /// <summary>
        /// Obtem todas as categorias cadastradas.
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _categoryApplication.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Obtem uma categoria específica pelo id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromBody] int id)
        {
            var result = await _categoryApplication.GetByIdAsync(id);
            return Ok(result);
        }

    }
}
