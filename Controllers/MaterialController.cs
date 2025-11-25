using AA1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AA1.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class MaterialController : ControllerBase
   {
    private static List<Material> material = new List<Material>();

    private readonly IMaterialRepository _repository;

    public MaterialController(IMaterialRepository repository)
        {
            _repository = repository;
        }
    
        [HttpGet]
        public async Task<ActionResult<List<Material>>> GetMaterial()
        {
            var materiales = await _repository.GetAllAsync();
            return Ok(materiales);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Material>> GetMaterial(int id)
        {
            var material = await _repository.GetByIdAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            return Ok(material);
        }

        [HttpPost]
        public async Task<ActionResult<Material>> CreateMaterial(Material material)
        {
            await _repository.AddAsync(material);
            return CreatedAtAction(nameof(GetMaterial), new { id = material.IdMaterial }, material);
        }

       [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterial(int id, Material updatedMaterial)
        {
            var existingMaterial = await _repository.GetByIdAsync(id);
            if (existingMaterial == null)
            {
                return NotFound();
            }

            // Actualizar el Mantenimiento existente
            existingMaterial.Nombre = updatedMaterial.Nombre;
            existingMaterial.Cantidad = updatedMaterial.Cantidad;
            existingMaterial.Disponibilidad = updatedMaterial.Disponibilidad;
            existingMaterial.IdPista = updatedMaterial.IdPista;
            existingMaterial.FechaActu = updatedMaterial.FechaActu;

            await _repository.UpdateAsync(existingMaterial);
            return NoContent();
        }

        ///Cambio necesario///
  
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