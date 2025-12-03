using Models;
using Microsoft.Data.SqlClient;

namespace AA1.Repositories
{
    public class MantenimientoRepository : IMantenimientoRepository
    {
        private readonly string _connectionString;
        private readonly IPistaRepository _pistaRepository;

        // SOLO inyecta dependencias resolvibles
        public MantenimientoRepository(IConfiguration configuration, IPistaRepository pistaRepository)
        {
            _connectionString = configuration.GetConnectionString("AA1") ?? throw new InvalidOperationException("Connection string 'AA1' not found");
            _pistaRepository = pistaRepository;
        }

        public async Task<List<Mantenimiento>> GetAllAsync()
        {
            var mantenimientos = new List<Mantenimiento>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idMantenimiento, nombre, tlfn, cif, idPista, correo FROM MANTENIMIENTOS";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var mantenimiento = new Mantenimiento
                            {
                                IdMantenimiento = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Tlfno = reader.GetInt32(2),
                                Cif = reader.GetInt32(3),
                                IdPista = await _pistaRepository.GetByIdAsync(reader.GetInt32(4)),
                                Correo = reader.GetString(5)
                            }; 

                            mantenimientos.Add(mantenimiento);
                        }
                    }
                }
            }
            return mantenimientos;
        }

        public async Task<Mantenimiento?> GetByIdAsync(int id)
        {
            Mantenimiento? mantenimiento = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idMantenimiento, nombre, tlfn, cif, idPista, correo FROM MANTENIMIENTOS WHERE idMantenimiento = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            mantenimiento = new Mantenimiento
                            {
                                IdMantenimiento = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Tlfno = reader.GetInt32(2),
                                Cif = reader.GetInt32(3),
                                IdPista = await _pistaRepository.GetByIdAsync(reader.GetInt32(4)),
                                Correo = reader.GetString(5)
                            };
                        }
                    }
                }
            }
            return mantenimiento;
        }

        public async Task AddAsync(Mantenimiento mantenimiento)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                
                string query = @"INSERT INTO MANTENIMIENTOS (nombre, tlfn, cif, idPista, correo) 
                                OUTPUT INSERTED.idMantenimiento
                                VALUES (@nombre, @tlfno, @cif, @idPista, @correo)";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", mantenimiento.Nombre);
                    command.Parameters.AddWithValue("@tlfno", mantenimiento.Tlfno);
                    command.Parameters.AddWithValue("@cif", mantenimiento.Cif);
                    command.Parameters.AddWithValue("@idPista", mantenimiento.IdPista?.IdPista ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@correo", mantenimiento.Correo);

                    // Obtener el ID generado
                    var newId = await command.ExecuteScalarAsync();
                    mantenimiento.IdMantenimiento = Convert.ToInt32(newId);
                }
            }
        }

        public async Task UpdateAsync(Mantenimiento mantenimiento)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE MANTENIMIENTOS SET nombre = @nombre, tlfn = @tlfno, cif = @cif, idPista = @idPista, correo = @correo WHERE idMantenimiento = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", mantenimiento.IdMantenimiento);
                    command.Parameters.AddWithValue("@nombre", mantenimiento.Nombre);
                    command.Parameters.AddWithValue("@tlfno", mantenimiento.Tlfno);
                    command.Parameters.AddWithValue("@cif", mantenimiento.Cif);
                    command.Parameters.AddWithValue("@idPista", mantenimiento.IdPista?.IdPista ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@correo", mantenimiento.Correo);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM MANTENIMIENTOS WHERE idMantenimiento = @Id";
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

                var query = @"
                    IF NOT EXISTS (SELECT 1 FROM MANTENIMIENTOS)
                    BEGIN
                        INSERT INTO MANTENIMIENTOS (nombre, tlfn, cif, idPista, correo)
                        VALUES 
                        (@nombre1, @tlfno1, @cif1, @idPista1, @correo1),
                        (@nombre2, @tlfno2, @cif2, @idPista2, @correo2)
                    END";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre1", "Revisión red");
                    command.Parameters.AddWithValue("@tlfno1", 152847563);
                    command.Parameters.AddWithValue("@cif1", 258);
                    command.Parameters.AddWithValue("@idPista1", 1);
                    command.Parameters.AddWithValue("@correo1", "mantenimiento@club.com");
                    
                    command.Parameters.AddWithValue("@nombre2", "Cambio focos");
                    command.Parameters.AddWithValue("@tlfno2", 611259566);
                    command.Parameters.AddWithValue("@cif2", 364);
                    command.Parameters.AddWithValue("@idPista2", 2);
                    command.Parameters.AddWithValue("@correo2", "soporte@club.com");

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}