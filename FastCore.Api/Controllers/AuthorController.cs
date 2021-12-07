using FastCore.Application;
using FastCore.Application.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FastCore.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;
        private readonly IAuthorApplication _authorApplication;

        public AuthorController(ILogger<AuthorController> logger, IAuthorApplication authorApplication)
        {
            _logger = logger;
            _authorApplication = authorApplication;
        }

        /// <summary>
        /// Adiciona um autor novo.
        /// </summary>
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromBody] AuthorVm entity)
        {
            await _authorApplication.AddAsync(entity);
            return Ok();
        }

        /// <summary>
        /// Atualiza um autor existente.
        /// </summary>
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] AuthorVm entity)
        {
            await _authorApplication.UpdateAsync(entity);
            return Ok();
        }

        /// <summary>
        /// Apaga um autor existente.
        /// </summary>
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteByIdAsync([FromBody] AuthorVm entity)
        {
            await _authorApplication.DeleteByIdAsync(entity);
            return Ok();
        }

        /// <summary>
        /// Obtem todos os autores cadastrados.
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _authorApplication.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Obtem um autor específico pelo id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromBody] int id)
        {
            var result = await _authorApplication.GetByIdAsync(id);
            return Ok(result);
        }

    }
}
