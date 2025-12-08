using AA1.DTOs;
using AA1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AA1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MantenimientoController : ControllerBase
    {
        private readonly IMantenimientoRepository _repository;
        private readonly IPistaRepository _pistaRepository;

        public MantenimientoController(
            IMantenimientoRepository repository,
            IPistaRepository pistaRepository)
        {
            _repository = repository;
            _pistaRepository = pistaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<MantenimientoDto>>> GetMantenimiento()
        {
            var mantenimientos = await _repository.GetAllAsync();
            var mantenimientosDto = mantenimientos.Select(m => new MantenimientoDto
            {
                IdMantenimiento = m.IdMantenimiento,
                Nombre = m.Nombre,
                Tlfno = m.Tlfno,
                Cif = m.Cif,
                IdPista = m.IdPista?.IdPista ?? 0,
                Correo = m.Correo
            }).ToList();

            return Ok(mantenimientosDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MantenimientoDto>> GetMantenimiento(int id)
        {
            var mantenimiento = await _repository.GetByIdAsync(id);
            if (mantenimiento == null)
            {
                return NotFound();
            }

            var mantenimientoDto = new MantenimientoDto
            {
                IdMantenimiento = mantenimiento.IdMantenimiento,
                Nombre = mantenimiento.Nombre,
                Tlfno = mantenimiento.Tlfno,
                Cif = mantenimiento.Cif,
                IdPista = mantenimiento.IdPista?.IdPista ?? 0,
                Correo = mantenimiento.Correo
            };

            return Ok(mantenimientoDto);
        }

        [HttpPost]
        public async Task<ActionResult<MantenimientoDto>> CreateMantenimiento(CreateMantenimientoDto createDto)
        {
            var pista = await _pistaRepository.GetByIdAsync(createDto.IdPista);
            if (pista == null)
            {
                return BadRequest("Pista no encontrada");
            }

            var mantenimiento = new Mantenimiento
            {
                Nombre = createDto.Nombre,
                Tlfno = createDto.Tlfno,
                Cif = createDto.Cif,
                IdPista = pista,
                Correo = createDto.Correo
            };

            await _repository.AddAsync(mantenimiento);

            var mantenimientoDto = new MantenimientoDto
            {
                IdMantenimiento = mantenimiento.IdMantenimiento,
                Nombre = mantenimiento.Nombre,
                Tlfno = mantenimiento.Tlfno,
                Cif = mantenimiento.Cif,
                IdPista = mantenimiento.IdPista?.IdPista ?? 0,
                Correo = mantenimiento.Correo
            };

            return CreatedAtAction(nameof(GetMantenimiento), new { id = mantenimiento.IdMantenimiento }, mantenimientoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMantenimiento(int id, UpdateMantenimientoDto updateDto)
        {
            var existingMantenimiento = await _repository.GetByIdAsync(id);
            if (existingMantenimiento == null)
            {
                return NotFound();
            }

            var pista = await _pistaRepository.GetByIdAsync(updateDto.IdPista);
            if (pista == null)
            {
                return BadRequest("Pista no encontrada");
            }

            existingMantenimiento.Nombre = updateDto.Nombre;
            existingMantenimiento.Tlfno = updateDto.Tlfno;
            existingMantenimiento.Cif = updateDto.Cif;
            existingMantenimiento.IdPista = pista;
            existingMantenimiento.Correo = updateDto.Correo;

            await _repository.UpdateAsync(existingMantenimiento);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMantenimiento(int id)
        {
            var mantenimiento = await _repository.GetByIdAsync(id);
            if (mantenimiento == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}