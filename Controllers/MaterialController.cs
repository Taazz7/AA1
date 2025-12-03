using AA1.DTOs;
using AA1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AA1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialRepository _repository;
        private readonly IPistaRepository _pistaRepository;

        public MaterialController(
            IMaterialRepository repository,
            IPistaRepository pistaRepository)
        {
            _repository = repository;
            _pistaRepository = pistaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<MaterialDto>>> GetMaterial()
        {
            var materiales = await _repository.GetAllAsync();
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
            var material = await _repository.GetByIdAsync(id);
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
            var pista = await _pistaRepository.GetByIdAsync(createDto.IdPista);
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

            await _repository.AddAsync(material);

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
            var existingMaterial = await _repository.GetByIdAsync(id);
            if (existingMaterial == null)
            {
                return NotFound();
            }

            var pista = await _pistaRepository.GetByIdAsync(updateDto.IdPista);
            if (pista == null)
            {
                return BadRequest("Pista no encontrada");
            }

            existingMaterial.Nombre = updateDto.Nombre;
            existingMaterial.Cantidad = updateDto.Cantidad;
            existingMaterial.Disponibilidad = updateDto.Disponibilidad;
            existingMaterial.IdPista = pista;
            existingMaterial.FechaActu = updateDto.FechaActu;

            await _repository.UpdateAsync(existingMaterial);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await _repository.GetByIdAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}