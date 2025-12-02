using AA1.Repositories;
using AA1.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AA1.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class ReservaController : ControllerBase
   {
    private static List<Reserva> reserva = new List<Reserva>();

    private readonly IReservaRepository _repository;

    public ReservaController(IReservaRepository repository)
        {
            _repository = repository;
        }
    
        [HttpGet]
        public async Task<ActionResult<List<Reserva>>> GetReserva()
        {
            var reservas = await _repository.GetAllAsync();
            return Ok(reservas);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
            var reserva = await _repository.GetByIdAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            return Ok(reserva);
        }

        [HttpPost]
        public async Task<ActionResult<Reserva>> CreateReserva(Reserva reserva)
        {
            await _repository.AddAsync(reserva);
            return CreatedAtAction(nameof(GetReserva), new { id = reserva.IdReserva }, reserva);
        }

       [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReserva(int id, Reserva updatedReserva)
        {
            var existingReserva = await _repository.GetByIdAsync(id);
            if (existingReserva == null)
            {
                return NotFound();
            }

            // Actualizar el Reserva existente
            existingReserva.IdUsuario = updatedReserva.IdUsuario;
            existingReserva.IdPista = updatedReserva.IdPista;
            existingReserva.Fecha = updatedReserva.Fecha;
            existingReserva.Horas = updatedReserva.Horas;
            existingReserva.Precio = updatedReserva.Precio;

            await _repository.UpdateAsync(existingReserva);
            return NoContent();
        }

        ///Cambio necesario///
  
       [HttpDelete("{id}")]
       public async Task<IActionResult> DeleteReserva(int id)
       {
           var reserva = await _repository.GetByIdAsync(id);
           if (reserva == null)
           {
               return NotFound();
           }
           await _repository.DeleteAsync(id);
           return NoContent();
       }

   }
}