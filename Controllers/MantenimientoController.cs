using AA1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AA1.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class MantenimientoController : ControllerBase
   {
    private static List<Mantenimiento> mantenimiento = new List<Mantenimiento>();

    private readonly IMantenimientoRepository _repository;

    public MantenimientoController(IMantenimientoRepository repository)
        {
            _repository = repository;
        }
    
        [HttpGet]
        public async Task<ActionResult<List<Mantenimiento>>> GetMantenimiento()
        {
            var mantenimientos = await _repository.GetAllAsync();
            return Ok(mantenimientos);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Mantenimiento>> GetMantenimiento(int id)
        {
            var mantenimiento = await _repository.GetByIdAsync(id);
            if (mantenimiento == null)
            {
                return NotFound();
            }
            return Ok(mantenimiento);
        }

        [HttpPost]
        public async Task<ActionResult<Mantenimiento>> CreateMantenimiento(Mantenimiento mantenimiento)
        {
            await _repository.AddAsync(mantenimiento);
            return CreatedAtAction(nameof(GetMantenimiento), new { id = mantenimiento.IdMantenimiento }, mantenimiento);
        }

       [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMantenimiento(int id, Mantenimiento updatedMantenimiento)
        {
            var existingMantenimiento = await _repository.GetByIdAsync(id);
            if (existingMantenimiento == null)
            {
                return NotFound();
            }

            // Actualizar el Mantenimiento existente
            existingMantenimiento.Nombre = updatedMantenimiento.Nombre;
            existingMantenimiento.Tlfno = updatedMantenimiento.Tlfno;
            existingMantenimiento.Cif = updatedMantenimiento.Cif;
            existingMantenimiento.IdPista = updatedMantenimiento.IdPista;
            existingMantenimiento.Correo = updatedMantenimiento.Correo;

            await _repository.UpdateAsync(existingMantenimiento);
            return NoContent();
        }

        ///Cambio necesario///
  
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