using Microsoft.AspNetCore.Mvc;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotorcyclesController : ControllerBase
    {
        private readonly IMotorcycleRepository _repository;

        public MotorcyclesController(IMotorcycleRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMotorcycles()
        {
            var motorcycles = await _repository.GetAllAsync();
            return Ok(motorcycles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMotorcycle(int id)
        {
            var motorcycle = await _repository.GetByIdAsync(id);
            if (motorcycle == null)
            {
                return NotFound();
            }
            return Ok(motorcycle);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMotorcycle([FromBody] Motorcycle motorcycle)
        {
            await _repository.AddAsync(motorcycle);
            return CreatedAtAction(nameof(GetMotorcycle), new { id = motorcycle.Id }, motorcycle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMotorcycle(int id, [FromBody] Motorcycle motorcycle)
        {
            if (id != motorcycle.Id)
            {
                return BadRequest();
            }
            await _repository.UpdateAsync(motorcycle);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMotorcycle(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }

}
