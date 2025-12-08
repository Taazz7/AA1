using AA1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AA1.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class PistaController : ControllerBase
   {
    private static List<Pista> pista = new List<Pista>();

    private readonly IPistaRepository _repository;

    public PistaController(IPistaRepository repository)
        {
            _repository = repository;
        }
    
        [HttpGet]
        public async Task<ActionResult<List<Pista>>> GetPista()
        {
            var pistas = await _repository.GetAllAsync();
            return Ok(pistas);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Pista>> GetPista(int id)
        {
            var pista = await _repository.GetByIdAsync(id);
            if (pista == null)
            {
                return NotFound();
            }
            return Ok(pista);
        }

        [HttpPost]
        public async Task<ActionResult<Pista>> CreatePista(Pista pista)
        {
            await _repository.AddAsync(pista);
            return CreatedAtAction(nameof(GetPista), new { id = pista.IdPista }, pista);
        }

       [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePista(int id, Pista updatedPista)
        {
            var existingPista = await _repository.GetByIdAsync(id);
            if (existingPista == null)
            {
                return NotFound();
            }

            // Actualizar el Mantenimiento existente
            existingPista.Nombre = updatedPista.Nombre;
            existingPista.Tipo = updatedPista.Tipo;
            existingPista.Direccion = updatedPista.Direccion;
            existingPista.Activa = updatedPista.Activa;
            existingPista.PrecioHora = updatedPista.PrecioHora;

            await _repository.UpdateAsync(existingPista);
            return NoContent();
        }

        ///Cambio necesario///
  
       [HttpDelete("{id}")]
       public async Task<IActionResult> DeletePista(int id)
       {
           var pista = await _repository.GetByIdAsync(id);
           if (pista == null)
           {
               return NotFound();
           }
           await _repository.DeleteAsync(id);
           return NoContent();
       }

       [HttpGet("search")]
        public async Task<ActionResult<List<Pista>>> SearchPistas(
            [FromQuery] string? tipo,
            [FromQuery] bool? activa,
            [FromQuery] string? orderBy,
            [FromQuery] bool ascending = true)
        {
            var pistas = await _repository.GetAllFilteredAsync(tipo, activa, orderBy, ascending);
            return Ok(pistas);
        }

        /*Ejemplos de uso para postman: 
        # Filtrar por tipo
        GET /api/pista/search?tipo=Tenis

        # Filtrar por activa
        GET /api/pista/search?activa=true

        # Ordenar por precio (descendente)
        GET /api/pista/search?orderBy=precioHora&ascending=false

        # Ordenar por nombre (ascendente)
        GET /api/pista/search?orderBy=nombre&ascending=true

        # Combinar: pistas de tenis activas, ordenadas por precio
        GET /api/pista/search?tipo=Tenis&activa=true&orderBy=precioHora&ascending=false*/
   }
}