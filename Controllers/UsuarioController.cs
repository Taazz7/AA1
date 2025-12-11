using AA1.Repositories;
using AA1.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AA1.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class UsuarioController : ControllerBase
   {
    private static List<Usuario> usuario = new List<Usuario>();

    private readonly IUsuarioService _service;
    private readonly IReservaService _reservaService; 

    public UsuarioController(IUsuarioService service, IReservaService reservaService) 
        {
            _service = service;
            _reservaService = reservaService; 
        }
    
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetUsuario()
        {
            var usuarios = await _service.GetAllAsync();
            return Ok(usuarios);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        // ENDPOINT USUARIO PARA RESERVAS
        [HttpGet("{id}/reservas")]
        public async Task<ActionResult<List<Reserva>>> GetReservasByUsuario(int id)
        {
            // Verificar que el usuario existe
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new { mensaje = $"Usuario con ID {id} no encontrado" });
            }

            // Obtener las reservas del usuario
            var reservas = await _reservaService.GetByUsuarioIdAsync(id);
            return Ok(reservas);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
        {
            await _service.AddAsync(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuario);
        }

       [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, Usuario updatedUsuario)
        {
            var existingUsuario = await _service.GetByIdAsync(id);
            if (existingUsuario == null)
            {
                return NotFound();
            }

            // Actualizar el Usuario existente
            existingUsuario.Nombre = updatedUsuario.Nombre;
            existingUsuario.Apellido = updatedUsuario.Apellido;
            existingUsuario.Telefono = updatedUsuario.Telefono;
            existingUsuario.Direccion = updatedUsuario.Direccion;
            existingUsuario.FechaNac = updatedUsuario.FechaNac;

            await _service.UpdateAsync(existingUsuario);
            return NoContent();
        }
  
       [HttpDelete("{id}")]
       public async Task<IActionResult> DeleteUsuario(int id)
       {
           var usuario = await _service.GetByIdAsync(id);
           if (usuario == null)
           {
               return NotFound();
           }
           await _service.DeleteAsync(id);
           return NoContent();
       }

   }
}