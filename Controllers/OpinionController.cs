using AA1.DTOs;
using AA1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Models;

namespace AA1.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class OpinionController : ControllerBase
   {
    private static List<Opinion> opinion = new List<Opinion>();

    private readonly IOpinionRepository _repository;
    private readonly IPistaRepository _pistaRepository; 

    public OpinionController(IOpinionRepository repository, IPistaRepository pistaRepository) 
        {
            _repository = repository;
            _pistaRepository = pistaRepository; 
        }
    
        [HttpGet]
        public async Task<ActionResult<List<OpinionDto>>> GetOpinion()
        {
            var opiniones = await _repository.GetAllAsync();
            var opinionesDto = opiniones.Select(r => new OpinionDto
            {
                IdOpinion = r.IdOpinion,
                Nombre = r.Nombre,
                IdPista = r.IdPista?.IdPista ?? 0,
                Puntuacion = r.Puntuacion,
                FechaCrea = r.FechaCrea,
                Texto = r.Texto
            }).ToList();

            return Ok(opinionesDto);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<OpinionDto>> GetOpinion(int id)
        {
            var opinion = await _repository.GetByIdAsync(id);
            if (opinion == null)
            {
                return NotFound();
            }

            var opinionDto = new OpinionDto
            {
                IdOpinion = opinion.IdOpinion,
                Nombre = opinion.Nombre,
                IdPista = opinion.IdPista?.IdPista ?? 0,
                Puntuacion = opinion.Puntuacion,
                FechaCrea = opinion.FechaCrea,
                Texto = opinion.Texto
            };

            return Ok(opinionDto);
        }

        

        [HttpGet("search")]
        public async Task<ActionResult<List<OpinionDto>>> SearchOpinionesPuntuacion(
            [FromQuery] int? puntuacion,            
            [FromQuery] string? orderBy,
            [FromQuery] bool ascending = true)
        {
            var opiniones = await _repository.GetAllFilteredPuntuAsync(puntuacion, orderBy, ascending);
            return Ok(opiniones);
        }

        [HttpPost]
        public async Task<ActionResult<OpinionDto>> CreateOpinion(CreateOpinionDto opinionDto)
        {
            var pista = await _pistaRepository.GetByIdAsync(opinionDto.IdPista);
            if (pista == null)
            {
                return BadRequest("Pista no encontrada");
            }
            var opinion = new Opinion
            {
              
                Nombre = opinionDto.Nombre,
                Texto = opinionDto.Texto,
                Puntuacion = opinionDto.Puntuacion,
                FechaCrea = opinionDto.FechaCrea,
                IdPista = pista
            };

            await _repository.AddAsync(opinion);

            var createDto = new OpinionDto
            {
                IdOpinion = opinion.IdOpinion,
                Nombre = opinion.Nombre,
                IdPista = opinion.IdPista?.IdPista ?? 0,
                Texto = opinion.Texto,
                Puntuacion = opinion.Puntuacion,
                FechaCrea = opinion.FechaCrea
            };

            return CreatedAtAction(nameof(GetOpinion), new { id = opinion.IdOpinion }, createDto);
        }

       [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOpinion(int id, OpinionDto updatedOpinionDto)
        {
            var existingOpinion = await _repository.GetByIdAsync(id);
            if (existingOpinion == null)
            {
                return NotFound();
            }

             var pista = await _pistaRepository.GetByIdAsync(updatedOpinionDto.IdPista);
            if (pista == null)
            {
                return BadRequest("Pista no encontrada");
            }

            // Actualizar Opinion existente
            existingOpinion.Nombre = updatedOpinionDto.Nombre;
            existingOpinion.Texto = updatedOpinionDto.Texto;
            existingOpinion.Puntuacion = updatedOpinionDto.Puntuacion;
            existingOpinion.FechaCrea = updatedOpinionDto.FechaCrea;
            existingOpinion.IdPista = pista;

            await _repository.UpdateAsync(existingOpinion);
            return NoContent();
        }
  
       [HttpDelete("{id}")]
       public async Task<IActionResult> DeleteOpinion(int id)
       {
           var opinion = await _repository.GetByIdAsync(id);
           if (opinion == null)
           {
               return NotFound();
           }
           await _repository.DeleteAsync(id);
           return NoContent();
       }

   }
}