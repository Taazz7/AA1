using Microsoft.Data.SqlClient;
using Models;
using AA1.Repositories;

namespace AA1.Repositories
{
    public class ReseñaRepository : IReseñaRepository
    {
        private readonly string _connectionString;
        private readonly IReservaRepository _reservaRepository;

        public ReseñaRepository (IConfiguration configuration, IReservaRepository reservaRepository)
        {
            _connectionString = configuration.GetConnectionString("AA1") ?? "Not found";
            _reservaRepository = reservaRepository;
        }
        //SELECT 'PISTAS', COUNT(*) FROM PISTAS
        public async Task<int> GetCountAsync()
        {
            var reseñas = new List<Reseña>();
            var COUNT = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idReseña, idUsuario, idPista, valoracion, titulo, descripcion, fecha FROM RESEÑAS";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {   
                        while (await reader.ReadAsync())
                        {
                            var reseña = new Reseña
                            {
                                IdReseña = reader.GetInt32(0),
                                IdReserva = await _reservaRepository.GetByIdAsync(reader.GetInt32(1)),
                                Valoracion = reader.GetInt32(2),
                                Titulo = reader.GetString(3),
                                Descripcion = reader.GetString(4),
                                Fecha = reader.GetDateTime(5)
                            };
                            COUNT++;
                            reseñas.Add(reseña);
                            
                        }
                    }
                }
            }
            return COUNT;
        }
        public async Task<List<Reseña>> GetAllAsync()
        {
            var reseñas = new List<Reseña>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idReseña, idUsuario, idPista, valoracion, titulo, descripcion, fecha FROM RESEÑAS";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var reseña = new Reseña
                            {
                                IdReseña = reader.GetInt32(0),
                                IdReserva = await _reservaRepository.GetByIdAsync(reader.GetInt32(1)),
                                Valoracion = reader.GetInt32(2),
                                Titulo = reader.GetString(3),
                                Descripcion = reader.GetString(4),
                                Fecha = reader.GetDateTime(5)
                            };

                            reseñas.Add(reseña);
                        }
                    }
                }
            }
            return reseñas;
        }

        public async Task<Reseña> GetByIdAsync(int idReseña)
        {
            Reseña reseña = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idReseña, idUsuario, idPista, valoracion, titulo, descripcion, fecha FROM RESEÑAS WHERE idReseña = @IdReseña";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdReseña", idReseña);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            reseña = new Reseña
                            {
                                IdReseña = reader.GetInt32(0),
                                IdReserva = await _reservaRepository.GetByIdAsync(reader.GetInt32(1)),
                                Valoracion = reader.GetInt32(2),
                                Titulo = reader.GetString(3),
                                Descripcion = reader.GetString(4),
                                Fecha = reader.GetDateTime(5)
                            };
                        }
                    }
                }
            }
            return reseña;
        }

        public async Task AddAsync(Reseña reseña)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // QUITAMOS IdPista del INSERT
                string query = @"INSERT INTO RESEÑAS (idReserva, Valoracion, Titulo, Descripcion, Fecha) 
                                OUTPUT INSERTED.idReseña
                                VALUES (@idReserva, @Valoracion, @Titulo, @Descripcion, @Fecha)";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idReserva", reseña.IdReserva?.IdReserva ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Valoracion", reseña.Valoracion);
                    command.Parameters.AddWithValue("@Titulo", reseña.Titulo);
                    command.Parameters.AddWithValue("@Descripcion", reseña.Descripcion);
                    command.Parameters.AddWithValue("@Fecha", reseña.Fecha);

                    var newId = await command.ExecuteScalarAsync();
                    reseña.IdReseña = Convert.ToInt32(newId);
                }
            }
        }



        public async Task UpdateAsync(Reseña reseña)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE RESEÑAS SET idReserva = @idReserva, Valoracion = @Valoracion, Titulo = @Titulo, Descripcion = @Descripcion, Fecha = @Fecha WHERE idReseña = @IdReseña";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idReserva", reseña.IdReserva);
                    command.Parameters.AddWithValue("@Valoracion", reseña.Valoracion);
                    command.Parameters.AddWithValue("@Titulo", reseña.Titulo);
                    command.Parameters.AddWithValue("@Descripcion", reseña.Descripcion);
                    command.Parameters.AddWithValue("@Fecha", reseña.Fecha);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int idReseña)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM RESEÑAS WHERE idReseña = @IdReseña";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdReseña", idReseña);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<Reseña> GetMinPuntAsync()
        {
            var reseña = new Reseña{};

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT MIN(valoracion) FROM RESEÑAS";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            reseña = new Reseña
                            {
                                IdReseña = reader.GetInt32(0),
                                IdReserva = await _reservaRepository.GetByIdAsync(reader.GetInt32(1)),
                                Valoracion = reader.GetInt32(2),
                                Titulo = reader.GetString(3),
                                Descripcion = reader.GetString(4),
                                Fecha = reader.GetDateTime(5)
                            };

                        }
                    }
                }
            }
            return reseña;
        }

        public async Task<Reseña> GetMinFechaAsync()
        {
            var reseña = new Reseña{};

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT MIN(fecha) FROM RESEÑAS";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            reseña = new Reseña
                            {
                                IdReseña = reader.GetInt32(0),
                                IdReserva = await _reservaRepository.GetByIdAsync(reader.GetInt32(1)),
                                Valoracion = reader.GetInt32(2),
                                Titulo = reader.GetString(3),
                                Descripcion = reader.GetString(4),
                                Fecha = reader.GetDateTime(5)
                            };

                        }
                    }
                }
            }
            return reseña;
        }


        public async Task<List<Reseña>> GetAllFilteredAsync(DateTime? fecha, int? valoracion, string? orderBy, bool ascending)
        {
            var reseñas = new List<Reseña>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idReseña, valoracion, titulo, descripcion, fecha FROM RESEÑAS WHERE 1=1";
                var parameters = new List<SqlParameter>();

                // Filtros
                if (valoracion.HasValue)
                {
                    query += " AND fecha = @valoracion";
                    parameters.Add(new SqlParameter("@valoracion", valoracion));
                }

                if (fecha.HasValue)
                {
                    query += " AND fecha = @Fecha";
                    parameters.Add(new SqlParameter("@Fecha", fecha.Value));
                }
                

                // Ordenación
                if (!string.IsNullOrWhiteSpace(orderBy))
                {
                    var validColumns = new[] { "Valoracion","Fecha" }; //En el enunciado pone solo Valoracion y Fecha
                    var orderByLower = orderBy.ToLower();
                    
                    if (validColumns.Contains(orderByLower))
                    {
                        var direction = ascending ? "ASC" : "DESC";
                        query += $" ORDER BY {orderByLower} {direction}";
                    }else
                {
                    query += " ORDER BY nombre ASC";
                }
                }
                

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var reseña = new Reseña
                            {
                                IdReseña = reader.GetInt32(0),
                                Valoracion = reader.GetInt32(1),
                                Titulo = reader.GetString(2),
                                Descripcion = reader.GetString(3),
                                Fecha = reader.GetDateTime(4)
                            };

                            reseñas.Add(reseña);
                        }
                    }
                }
            }
            return reseñas;
        }


        public async Task InicializarDatosAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Comando SQL para insertar datos iniciales
                var query = @"
                    INSERT INTO RESEÑAS (idUsuario, idPista, Valoracion, Titulo, Descripcion, Fecha)
                    VALUES 
                    (@idReserva1, @Valoracion1, @Titulo1, @Descripcion1, @Fecha1),
                    (@idReserva2, @Valoracion2, @Titulo2, @Descripcion2, @Fecha2)";

                using (var command = new SqlCommand(query, connection))
                {
                    // Parámetros para el primera reseña
                    command.Parameters.AddWithValue("@idReserva1", 1);
                    command.Parameters.AddWithValue("@Valoracion1", 5);
                    command.Parameters.AddWithValue("@Titulo1", "Pista Tenis");
                    command.Parameters.AddWithValue("@Descripcion1", "La pista esta bien cuidada y en muy buen estado");
                    command.Parameters.AddWithValue("@Fecha1", DateTime.Today);

                    // Parámetros para el segunda reseña
                    command.Parameters.AddWithValue("@idReserva2", 2);
                    command.Parameters.AddWithValue("@Valoracion2", 1);
                    command.Parameters.AddWithValue("@Titulo2", "Pista Futbol");
                    command.Parameters.AddWithValue("@Descripcion2", "El cesped estaba muy alto se notaba que no lo regaban bien y se hacia muy dificil jugar" );
                    command.Parameters.AddWithValue("@Fecha2", DateTime.Today);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    // reseñas 1 user

public async Task<List<Reseña>> GetByReservaIdAsync(int idReserva)
{
    var reseñas = new List<Reseña>();

    using (var connection = new SqlConnection(_connectionString))
    {
        await connection.OpenAsync();

        string query = "SELECT idReseña, idReserva, valoracion, titulo, descripcion, fecha FROM RESEÑAS WHERE idReserva = @IdReserva";
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@IdReserva", idReserva);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var reseña = new Reseña
                    {
                        IdReseña = reader.GetInt32(0),
                        IdReserva = await _reservaRepository.GetByIdAsync(reader.GetInt32(1)),
                        Valoracion = reader.GetInt32(2),
                        Titulo = reader.GetString(3),
                        Descripcion = reader.GetString(4),
                        Fecha = reader.GetDateTime(5)
                    };

                    reseñas.Add(reseña);
                }
            }
        }
    }
    return reseñas;
}

    }

}