using System.Data;
using Microsoft.Data.SqlClient;
using Models;

namespace AA1.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly string _connectionString;
        private readonly int _idMaterial;
        private readonly string _nombre;
        private readonly int _cantidad;
        private readonly IPistaRepository _idPista;
        private readonly bool _disponibilidad;
        private readonly DateTime _fechaAct;

        public MaterialRepository(IConfiguration configuration, int idMaterial, string nombre, int cantidad, IPistaRepository idPista, bool disponibilidad, DateTime fechAct)
        {
             _connectionString = configuration.GetConnectionString("AA1") ?? "Not found";
            _idMaterial = idMaterial;
            _nombre = nombre;
            _cantidad = cantidad;
            _idPista = idPista;
            _disponibilidad = disponibilidad;
            _fechaAct = fechAct;
        }
        

        public async Task<List<Material>> GetAllAsync()
        {
            var materiales = new List<Material>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idMaterial, nombre, cantidad, disponibilidad, idPista, fechaAct FROM Materiales";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var material = new Material
                            {
                                IdMaterial = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Cantidad = reader.GetInt32(2),
                                Disponibilidad = reader.GetInt32(3),
                                IdPista = await _idPista.GetByIdAsync(reader.GetInt32(4)),
                                FechaActu = reader.GetDateTime(5)
                            }; 

                            materiales.Add(material);
                        }
                    }
                }
            }
            return materiales;
        }

        public async Task<Material> GetByIdAsync(int id)
        {
            Material material = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idMaterial, nombre, cantidad, disponibilidad, idPista, fechaAct FROM Materiales WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            material = new Material
                            {
                                IdMaterial = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Cantidad = reader.GetString(2),
                                Disponibilidad = reader.GetInt32(3),
                                IdPista = await _idPista.GetByIdAsync(reader.GetInt32(4)),
                                FechaActu = reader.GetDateTime(5)
                            };
                        }
                    }
                }
            }
            return material;
        }

        public async Task AddAsync(Material material)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO MATERIALES (idMaterial, nombre, cantidad, disponibilidad, idPista, fechaAct) VALUES (@idMaterial, @nombre, @cantidad, @disponibilidad, @idPista, @fechaAct)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idMaterial", material.IdMaterial);
                    command.Parameters.AddWithValue("@nombre", material.Nombre);
                    command.Parameters.AddWithValue("@cantidad", material.Cantidad);
                    command.Parameters.AddWithValue("@disponibilidad", material.Disponibilidad);
                    command.Parameters.AddWithValue("@idPista", material.IdPista);
                    command.Parameters.AddWithValue("@fechaAct", material.FechaActu);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Material material)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE MATERIALES SET idMaterial = @idMaterial, nombre = @nombre, cantidad = @cantidad, disponibilidad = @disponibilidad, idPista =@idPista, correo = @correo WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idMantenimiento", material.IdMaterial);
                    command.Parameters.AddWithValue("@nombre", material.Nombre);
                    command.Parameters.AddWithValue("@cantidad", material.Cantidad);
                    command.Parameters.AddWithValue("@disponibilidad", material.Disponibilidad);
                    command.Parameters.AddWithValue("@idPista", material.IdPista);
                    command.Parameters.AddWithValue("@fechaAct", material.FechaActu);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM MATERIALES WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

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
                    INSERT INTO MATERIALES (idMaterial, nombre, cantidad, disponibilidad, idPista, fechaAct)
                    VALUES 
                    (@idMaterial1, @nombre1, @cantidad1, @disponibilidad1, @idPista1, @fechaAct1),
                    (@idMaterial2, @nombre2, @cantidad2, @disponibilidad2, @idPista2, @fechaAct2)";

                using (var command = new SqlCommand(query, connection))
                {
                    // Parámetros para el primer bebida
                    command.Parameters.AddWithValue("@idMaterial1", 1);
                    command.Parameters.AddWithValue("@nombre1", "Pelota Baloncesto");
                    command.Parameters.AddWithValue("@cantidad1", 35);
                    command.Parameters.AddWithValue("@disponibilidad1", 1);
                    command.Parameters.AddWithValue("@idPista1", 1);
                    command.Parameters.AddWithValue("@fechaAct1", DateTime.Now);
                    

                    // Parámetros para el segundo bebida
                    command.Parameters.AddWithValue("@idMaterial2", 2);
                    command.Parameters.AddWithValue("@nombre2", "Banderín de Corner");
                    command.Parameters.AddWithValue("@cantidad2", 4);
                    command.Parameters.AddWithValue("@disponibilidad2", 0);
                    command.Parameters.AddWithValue("@idPista2", 2);
                    command.Parameters.AddWithValue("@fechaAct2", DateTime.Now);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


    }

}