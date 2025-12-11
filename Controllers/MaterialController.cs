using AA1.DTOs;
using AA1.Repositories;
using AA1.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AA1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _service;
        private readonly IPistaService _pistaService;

        public MaterialController(
            IMaterialService service,
            IPistaService pistaService)
        {
            _service = service;
            _pistaService = pistaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MaterialDto>>> GetMaterial()
        {
            var materiales = await _service.GetAllAsync();
            var materialesDto = materiales.Select(m => new MaterialDto
            {
                IdMaterial = m.IdMaterial,
                Nombre = m.Nombre,
                Cantidad = m.Cantidad,
                Disponibilidad = m.Disponibilidad,
                IdPista = m.IdPista?.IdPista ?? 0,
                FechaActu = m.FechaActu
            }).ToList();

            return Ok(materialesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialDto>> GetMaterial(int id)
        {
            var material = await _service.GetByIdAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            var materialDto = new MaterialDto
            {
                IdMaterial = material.IdMaterial,
                Nombre = material.Nombre,
                Cantidad = material.Cantidad,
                Disponibilidad = material.Disponibilidad,
                IdPista = material.IdPista?.IdPista ?? 0,
                FechaActu = material.FechaActu
            };

            return Ok(materialDto);
        }

        [HttpPost]
        public async Task<ActionResult<MaterialDto>> CreateMaterial(CreateMaterialDto createDto)
        {
            var pista = await _pistaService.GetByIdAsync(createDto.IdPista);
            if (pista == null)
            {
                return BadRequest("Pista no encontrada");
            }

            var material = new Material
            {
                Nombre = createDto.Nombre,
                Cantidad = createDto.Cantidad,
                Disponibilidad = createDto.Disponibilidad,
                IdPista = pista,
                FechaActu = createDto.FechaActu
            };

            await _service.AddAsync(material);

            var materialDto = new MaterialDto
            {
                IdMaterial = material.IdMaterial,
                Nombre = material.Nombre,
                Cantidad = material.Cantidad,
                Disponibilidad = material.Disponibilidad,
                IdPista = material.IdPista?.IdPista ?? 0,
                FechaActu = material.FechaActu
            };

            return CreatedAtAction(nameof(GetMaterial), new { id = material.IdMaterial }, materialDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterial(int id, UpdateMaterialDto updateDto)
        {
            var existingMaterial = await _service.GetByIdAsync(id);
            if (existingMaterial == null)
            {
                return NotFound();
            }

            var pista = await _pistaService.GetByIdAsync(updateDto.IdPista);
            if (pista == null)
            {
                return BadRequest("Pista no encontrada");
            }

            existingMaterial.Nombre = updateDto.Nombre;
            existingMaterial.Cantidad = updateDto.Cantidad;
            existingMaterial.Disponibilidad = updateDto.Disponibilidad;
            existingMaterial.IdPista = pista;
            existingMaterial.FechaActu = updateDto.FechaActu;

            await _service.UpdateAsync(existingMaterial);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await _service.GetByIdAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}