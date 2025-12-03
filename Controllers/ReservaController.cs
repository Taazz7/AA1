using AA1.DTOs;
using AA1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AA1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaRepository _repository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPistaRepository _pistaRepository;

        public ReservaController(
            IReservaRepository repository,
            IUsuarioRepository usuarioRepository,
            IPistaRepository pistaRepository)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
            _pistaRepository = pistaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReservaDto>>> GetReserva()
        {
            var reservas = await _repository.GetAllAsync();
            var reservasDto = reservas.Select(r => new ReservaDto
            {
                IdReserva = r.IdReserva,
                IdUsuario = r.IdUsuario?.IdUsuario ?? 0,
                IdPista = r.IdPista?.IdPista ?? 0,
                Fecha = r.Fecha,
                Horas = r.Horas,
                Precio = r.Precio
            }).ToList();

            return Ok(reservasDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservaDto>> GetReserva(int id)
        {
            var reserva = await _repository.GetByIdAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            var reservaDto = new ReservaDto
            {
                IdReserva = reserva.IdReserva,
                IdUsuario = reserva.IdUsuario?.IdUsuario ?? 0,
                IdPista = reserva.IdPista?.IdPista ?? 0,
                Fecha = reserva.Fecha,
                Horas = reserva.Horas,
                Precio = reserva.Precio
            };

            return Ok(reservaDto);
        }

        [HttpPost]
        public async Task<ActionResult<ReservaDto>> CreateReserva(CreateReservaDto createDto)
        {
            // Validar que existan el usuario y la pista
            var usuario = await _usuarioRepository.GetByIdAsync(createDto.IdUsuario);
            if (usuario == null)
            {
                return BadRequest("Usuario no encontrado");
            }

            var pista = await _pistaRepository.GetByIdAsync(createDto.IdPista);
            if (pista == null)
            {
                return BadRequest("Pista no encontrada");
            }

            var reserva = new Reserva
            {
              
                IdUsuario = usuario,
                IdPista = pista,
                Fecha = createDto.Fecha,
                Horas = createDto.Horas,
                Precio = createDto.Precio
            };

            await _repository.AddAsync(reserva);

            var reservaDto = new ReservaDto
            {
                IdReserva = reserva.IdReserva,
                IdUsuario = reserva.IdUsuario?.IdUsuario ?? 0,
                IdPista = reserva.IdPista?.IdPista ?? 0,
                Fecha = reserva.Fecha,
                Horas = reserva.Horas,
                Precio = reserva.Precio
            };

            return CreatedAtAction(nameof(GetReserva), new { id = reserva.IdReserva }, reservaDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReserva(int id, UpdateReservaDto updateDto)
        {
            var existingReserva = await _repository.GetByIdAsync(id);
            if (existingReserva == null)
            {
                return NotFound();
            }

            // Validar que existan el usuario y la pista
            var usuario = await _usuarioRepository.GetByIdAsync(updateDto.IdUsuario);
            if (usuario == null)
            {
                return BadRequest("Usuario no encontrado");
            }

            var pista = await _pistaRepository.GetByIdAsync(updateDto.IdPista);
            if (pista == null)
            {
                return BadRequest("Pista no encontrada");
            }

            existingReserva.IdUsuario = usuario;
            existingReserva.IdPista = pista;
            existingReserva.Fecha = updateDto.Fecha;
            existingReserva.Horas = updateDto.Horas;
            existingReserva.Precio = updateDto.Precio;

            await _repository.UpdateAsync(existingReserva);
            return NoContent();
        }

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