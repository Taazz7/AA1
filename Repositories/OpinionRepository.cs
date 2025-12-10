
using System.Numerics;
using Microsoft.Data.SqlClient;
using Models;
using AA1.DTOs;

namespace AA1.Repositories
{
    public class OpinionRepository : IOpinionRepository
    {
        private readonly string _connectionString;
        private readonly IPistaRepository _pistarepository;

        public OpinionRepository(IConfiguration configuration, IPistaRepository pistarepository)
        {
             _connectionString = configuration.GetConnectionString("AA1") ?? "Not found";
             _pistarepository = pistarepository;
        }
        

        public async Task<List<Opinion>> GetAllAsync()
        {
            var opiniones = new List<Opinion>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idOpinion, nombre, texto, puntuacion, fechaCrea, idPista FROM OPINIONES";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var opinion = new Opinion
                            {
                                IdOpinion = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Texto = reader.GetString(2),
                                Puntuacion = reader.GetInt32(3),
                                FechaCrea = reader.GetDateTime(4),
                                IdPista = await _pistarepository.GetByIdAsync(reader.GetInt32(5))
                                
                            }; 

                            opiniones.Add(opinion);
                        }
                    }
                }
            }
            return opiniones;
        }

        public async Task<Opinion> GetByIdAsync(int idOpinion)
        {
            Opinion opinion = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idOpinion, nombre, texto, puntuacion, fechaCrea, idPista FROM OPINIONES WHERE IdOpinion = @IdOpinion";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdOpinion", idOpinion);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            opinion = new Opinion
                            {
                                IdOpinion = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Texto = reader.GetString(2),
                                Puntuacion = reader.GetInt32(3),
                                FechaCrea = reader.GetDateTime(4),
                                IdPista = await _pistarepository.GetByIdAsync(reader.GetInt32(5))
                            };
                        }
                    }
                }
            }
            return opinion;
        }

        public async Task AddAsync(Opinion opinion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // QUITAMOS id del INSERT
                string query = @"INSERT INTO OPINIONES (nombre, texto, puntuacion, fechaCrea, idPista) 
                                OUTPUT INSERTED.idOpinion
                                VALUES (@nombre, @texto, @puntuacion, @fechaCrea, @idPista)";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", opinion.Nombre);
                    command.Parameters.AddWithValue("@texto", opinion.Texto);
                    command.Parameters.AddWithValue("@puntuacion", opinion.Puntuacion);
                    command.Parameters.AddWithValue("@fechaCrea", opinion.FechaCrea);
                    command.Parameters.AddWithValue("@idPista", opinion.IdPista);

                    var newId = await command.ExecuteScalarAsync();
                    opinion.IdOpinion = Convert.ToInt32(newId);
                }
            }
        }

        public async Task UpdateAsync(Opinion opinion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE OPINIONES SET idOpinion = @idOpinion, nombre = @nombre, texto = @texto, puntuacion = @puntuacion, fechaCrea =@fechaCrea, idPista = @idPista WHERE IdOpinion = @IdOpinion";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idOpinion", opinion.IdOpinion);
                    command.Parameters.AddWithValue("@nombre", opinion.Nombre);
                    command.Parameters.AddWithValue("@texto", opinion.Texto);
                    command.Parameters.AddWithValue("@puntuacion", opinion.Puntuacion);
                    command.Parameters.AddWithValue("@fechaCrea", opinion.FechaCrea);
                    command.Parameters.AddWithValue("@idPista", opinion.IdPista);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int idOpinion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM OPINIONES WHERE IdOpinion = @IdOpinion";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdOpinion", idOpinion);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


        public async Task<List<OpinionDto>> GetAllFilteredPuntuAsync(int? puntuacion, string? orderBy, bool ascending)
        {
            var opiniones = new List<OpinionDto>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT nombre, texto, puntuacion, fechaCrea FROM OPINIONES WHERE 1=1";
                var parameters = new List<SqlParameter>();

                // Filtros
                if (puntuacion>0)
                {
                    query += " AND puntuacion = @Puntuacion";
                    parameters.Add(new SqlParameter("@Puntuacion", puntuacion));
                }  

                // Ordenación
                if (!string.IsNullOrWhiteSpace(orderBy))
                {
                    var validColumns = new[] { "nombre", "texto","puntuacion", "fechaCrea" };
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
                            /*var opinion = new Opinion
                            {
                                IdOpinion = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Texto = reader.GetString(2),
                                Puntuacion = reader.GetInt32(3),
                                FechaCrea = reader.GetDateTime(4)
                               
                            };*/

                            var opinionDto = new OpinionDto
                            {
                                IdOpinion = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Texto = reader.GetString(2),
                                Puntuacion = reader.GetInt32(3),
                                FechaCrea = reader.GetDateTime(4), 
                               // IdPista = reader.GetInt32(5)
                            };


                            opiniones.Add(opinionDto);
                        }
                    }
                }
            }
            return opiniones;
        }

        



        public async Task InicializarDatosAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Comando SQL para insertar datos iniciales
                var query = @"
                    INSERT INTO OPINIONES ( nombre, texto, puntuacion, fechaCrea, idPista)
                    VALUES 
                    (@nombre1, @texto1, @puntuacion1, @fechaCrea1, @idPista1),
                    (@nombre2, @texto2, @puntuacion2, @fechaCrea2, @idPista2)";

                using (var command = new SqlCommand(query, connection))
                {
                    // Parámetros para el primer bebida
                    command.Parameters.AddWithValue("@nombre1", "Ana");
                    command.Parameters.AddWithValue("@texto1", "texto ejemplo");
                    command.Parameters.AddWithValue("@puntuacion1", 2);
                    command.Parameters.AddWithValue("@fechaCrea1", "1990-05-15");
                    command.Parameters.AddWithValue("@idPista1", 2);
                    

                    // Parámetros para el segundo bebida
                    command.Parameters.AddWithValue("@nombre2", "Luis");
                    command.Parameters.AddWithValue("@texto2", "apruebame pls");
                    command.Parameters.AddWithValue("@puntuacion2", 5);
                    command.Parameters.AddWithValue("@fechaCrea2", "1985-11-30");
                    command.Parameters.AddWithValue("@idPista2", 2);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        
    }

}