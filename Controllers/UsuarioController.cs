using AA1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AA1.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class UsuarioController : ControllerBase
   {
    private static List<Usuario> usuario = new List<Usuario>();

    private readonly IUsuarioRepository _repository;

    public UsuarioController(IUsuarioRepository repository)
        {
            _repository = repository;
        }
    
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetUsuario()
        {
            var usuarios = await _repository.GetAllAsync();
            return Ok(usuarios);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
        {
            await _repository.AddAsync(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuario);
        }

       [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, Usuario updatedUsuario)
        {
            var existingUsuario = await _repository.GetByIdAsync(id);
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

            await _repository.UpdateAsync(existingUsuario);
            return NoContent();
        }

        ///Cambio necesario///
  
       [HttpDelete("{id}")]
       public async Task<IActionResult> DeleteUsuario(int id)
       {
           var usuario = await _repository.GetByIdAsync(id);
           if (usuario == null)
           {
               return NotFound();
           }
           await _repository.DeleteAsync(id);
           return NoContent();
       }

   }
}