using AA1.DTOs;
using AA1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AA1.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class ReseñaController : ControllerBase
   {
    private static List<Reseña> reseña = new List<Reseña>();

    private readonly IReseñaRepository _repository;
    private readonly IReservaRepository _reservaRepository;

    public ReseñaController(
        IReseñaRepository repository,
        IReservaRepository reservaRepository
        )
        {
            _repository = repository;
            _reservaRepository = reservaRepository;
        }
    
        [HttpGet]
        public async Task<ActionResult<List<Reseña>>> GetReseña()
        {
            var reseñas = await _repository.GetAllAsync();
            var reseñasDTO = reseñas.Select(r => new ReseñaDto
            { 
                IdReseña = r.IdReseña,
                IdReserva = r.IdReserva?.IdReserva ?? 0,
                Valoracion = r.Valoracion,
                Titulo = r.Titulo,
                Descripcion = r.Descripcion,
                Fecha = r.Fecha
            }).ToList();
            return Ok(reseñas);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Reseña>> GetReseña(int id)
        {
            var reseña = await _repository.GetByIdAsync(id);
            if (reseña == null)
            {
                return NotFound();
            }

            var reseñaDto = new ReseñaDto
            { 
                IdReseña = reseña.IdReseña,
                IdReserva = reseña.IdReserva?.IdReserva ?? 0,
                Valoracion = reseña.Valoracion,
                Titulo = reseña.Titulo,
                Descripcion = reseña.Descripcion,
                Fecha = reseña.Fecha
            };
            return Ok(reseña);
        }

        [HttpPost]
        public async Task<ActionResult<Reseña>> CreateReseña(CreateReseñaDto createDto)
        {
            // Validar que existan el usuario y la pista
            var reserva = await _reservaRepository.GetByIdAsync(createDto.IdReserva);
            if (reserva == null)
            {
                return BadRequest("Usuario no encontrado");
            }
            var reseña = new Reseña
            {
                IdReserva = reserva,
                Fecha = createDto.Fecha,
                Valoracion = createDto.Valoracion,
                Titulo = createDto.Titulo,
                Descripcion = createDto.Descripcion
            };

            await _repository.AddAsync(reseña);

            var reseñaDto = new ReseñaDto
            { 
                IdReseña = reseña.IdReseña,
                IdReserva = reseña.IdReserva?.IdReserva ?? 0,
                Valoracion = reseña.Valoracion,
                Titulo = reseña.Titulo,
                Descripcion = reseña.Descripcion,
                Fecha = reseña.Fecha
            };
            return CreatedAtAction(nameof(GetReseña), new { id = reseña.IdReseña }, reseñaDto);
        }

       [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReseña(int id, UpdateReseñaDto updateDto)
        {
            var existingReseña = await _repository.GetByIdAsync(id);
            if (existingReseña == null)
            {
                return NotFound();
            }
            // Validar que existan el usuario y la pista
            var reserva = await _reservaRepository.GetByIdAsync(updateDto.IdReserva);
            if (reserva == null)
            {
                return BadRequest("Usuario no encontrado");
            }

            // Actualizar el Mantenimiento existente
            existingReseña.IdReserva = reserva;
            existingReseña.Valoracion = updateDto.Valoracion;
            existingReseña.Titulo = updateDto.Titulo;
            existingReseña.Descripcion = updateDto.Descripcion;
            existingReseña.Fecha = updateDto.Fecha;

            await _repository.UpdateAsync(existingReseña);
            return NoContent();
        }

        ///Cambio necesario///
  
       [HttpDelete("{id}")]
       public async Task<IActionResult> DeleteReseña(int id)
       {
           var reseña = await _repository.GetByIdAsync(id);
           if (reseña == null)
           {
               return NotFound();
           }
           await _repository.DeleteAsync(id);
           return NoContent();
       }

       [HttpGet("search")]
        public async Task<ActionResult<List<Reseña>>> SearchReseñas(
            [FromQuery] DateTime? fecha,
            [FromQuery] int? valoracion,
            [FromQuery] string? orderBy,
            [FromQuery] bool ascending = true)
        {
            var reseñas = await _repository.GetAllFilteredAsync(fecha, valoracion, orderBy, ascending);
            return Ok(reseñas);
                }   
        [HttpGet("TotalOpiniones")]
        public async Task<ActionResult<Reseña>> TotalReseña()
        {
            var reseñas = await _repository.GetCountAsync();
            
            return Ok(reseñas);
        }
        [HttpGet("PuntuacionMinima")]
        public async Task<ActionResult<Reseña>> MinPuntReseña()
        {
            var reseñas = await _repository.GetMinPuntAsync();
            
            return Ok(reseñas);
        }
        [HttpGet("FechaMinima")]
        public async Task<ActionResult<Reseña>> MinFechaReseña()
        {
            var reseñas = await _repository.GetMinFechaAsync();
            
            return Ok(reseñas);
        }
            
    }
}

        
