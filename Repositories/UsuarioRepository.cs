
using Microsoft.Data.SqlClient;
using Models;

namespace AA1.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
             _connectionString = configuration.GetConnectionString("AA1") ?? "Not found";
        }
        

        public async Task<List<Usuario>> GetAllAsync()
        {
            var usuarios = new List<Usuario>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idUsuario, nombre, apellido, telefono, direccion, fechaNac FROM USUARIOS";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                Telefono = reader.GetInt32(3),
                                Direccion = reader.GetString(4),
                                FechaNac = reader.GetDateTime(5)
                            }; 

                            usuarios.Add(usuario);
                        }
                    }
                }
            }
            return usuarios;
        }

        public async Task<Usuario> GetByIdAsync(int idUsuario)
        {
            Usuario usuario = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idUsuario, nombre, apellido, telefono, direccion, fechaNac FROM USUARIOS WHERE IdUsuario = @IdUsuario";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                Telefono = reader.GetInt32(3),
                                Direccion = reader.GetString(4),
                                FechaNac = reader.GetDateTime(5)
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        public async Task AddAsync(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // QUITAMOS idUsuario del INSERT
                string query = @"INSERT INTO USUARIOS (nombre, apellido, telefono, direccion, fechaNac) 
                                OUTPUT INSERTED.idUsuario
                                VALUES (@nombre, @apellido, @telefono, @direccion, @fechaNac)";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@apellido", usuario.Apellido);
                    command.Parameters.AddWithValue("@telefono", usuario.Telefono);
                    command.Parameters.AddWithValue("@direccion", usuario.Direccion);
                    command.Parameters.AddWithValue("@fechaNac", usuario.FechaNac);

                    var newId = await command.ExecuteScalarAsync();
                    usuario.IdUsuario = Convert.ToInt32(newId);
                }
            }
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE USUARIOS SET idUsuario = @idUsuario, nombre = @nombre, apellido = @apellido, telefono = @telefono, direccion =@direccion, fechaNac = @fechaNac WHERE IdUsuario = @IdUsuario";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUsuario", usuario.IdUsuario);
                    command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@apellido", usuario.Apellido);
                    command.Parameters.AddWithValue("@telefono", usuario.Telefono);
                    command.Parameters.AddWithValue("@direccion", usuario.Direccion);
                    command.Parameters.AddWithValue("@fechaNac", usuario.FechaNac);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int idUsuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM USUARIOS WHERE IdUsuario = @IdUsuario";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


        public async Task InicializarDatosAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Comando SQL para insertar datos iniciales
                var query = @"
                    INSERT INTO USUARIOS (idUsuario, nombre, apellido, telefono, direccion, fechaNac)
                    VALUES 
                    (@idUsuario1, @nombre1, @apellido1, @telefono1, @direccion1, @fechaNac1),
                    (@idUsuario2, @nombre2, @apellido2, @telefono2, @direccion2, @fechaNac2)";

                using (var command = new SqlCommand(query, connection))
                {
                    // Parámetros para el primer bebida
                    command.Parameters.AddWithValue("@idUsuario1", 1);
                    command.Parameters.AddWithValue("@nombre1", "Ana");
                    command.Parameters.AddWithValue("@apellido1", "García");
                    command.Parameters.AddWithValue("@telefono1", 600123456);
                    command.Parameters.AddWithValue("@direccion1", "Calle Mayor 10");
                    command.Parameters.AddWithValue("@fechaNac1", "1990-05-15");
                    

                    // Parámetros para el segundo bebida
                    command.Parameters.AddWithValue("@idUsuario2", 2);
                    command.Parameters.AddWithValue("@nombre2", "Luis");
                    command.Parameters.AddWithValue("@apellido2", "Martínez");
                    command.Parameters.AddWithValue("@telefono2", 600654321);
                    command.Parameters.AddWithValue("@direccion2", "Av. Goya 22");
                    command.Parameters.AddWithValue("@fechaNac2", "1985-11-30");

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


    }

}