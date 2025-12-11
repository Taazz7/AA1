using AA1.DTOs;
using AA1.Repositories;
using AA1.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AA1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MantenimientoController : ControllerBase
    {
        private readonly IMantenimientoService _service;
        private readonly IPistaService _pistaService;

        public MantenimientoController(
            IMantenimientoService service,
            IPistaService pistaService)
        {
            _service = service;
            _pistaService = pistaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MantenimientoDto>>> GetMantenimiento()
        {
            var mantenimientos = await _service.GetAllAsync();
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
            var mantenimiento = await _service.GetByIdAsync(id);
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
            var pista = await _pistaService.GetByIdAsync(createDto.IdPista);
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

            await _service.AddAsync(mantenimiento);

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
            var existingMantenimiento = await _service.GetByIdAsync(id);
            if (existingMantenimiento == null)
            {
                return NotFound();
            }

            var pista = await _pistaService.GetByIdAsync(updateDto.IdPista);
            if (pista == null)
            {
                return BadRequest("Pista no encontrada");
            }

            existingMantenimiento.Nombre = updateDto.Nombre;
            existingMantenimiento.Tlfno = updateDto.Tlfno;
            existingMantenimiento.Cif = updateDto.Cif;
            existingMantenimiento.IdPista = pista;
            existingMantenimiento.Correo = updateDto.Correo;

            await _service.UpdateAsync(existingMantenimiento);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMantenimiento(int id)
        {
            var mantenimiento = await _service.GetByIdAsync(id);
            if (mantenimiento == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}