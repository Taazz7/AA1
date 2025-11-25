
using Microsoft.Data.SqlClient;
using Models;

namespace AA1.Repositories
{
    public class MantenimientoRepository : IMantenimientoRepository
    {
        private readonly string _connectionString;
        private readonly int _idMantenimiento;
        private readonly string _nombre;
        private readonly int _tlfno;
        private readonly IPista _idPista;
        private readonly int _cif;
        private readonly string _correo;

        public MantenimientoRepository(IConfiguration configuration, int idMantenimiento, string nombre, int tlfno, Pista idPista, int cif, string correo)
        {
             _connectionString = configuration.GetConnectionString("AA1") ?? "Not found";
            _idMantenimiento = idMantenimiento;
            _nombre = nombre;
            _tlfno = tlfno;
            _idPista = idPista;
            _cif = cif;
            _correo = correo;
        }
        

        public async Task<List<Mantenimiento>> GetAllAsync()
        {
            var mantenimientos = new List<Mantenimiento>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idMantenimiento, nombre, tlfno, cif, idPista, correo FROM Mantenimiento";
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
                                IdPista = await _idPista.GetByIdAsync(reader.GetInt32(4)),
                                Correo = reader.GetString(5)
                            }; 

                            mantenimientos.Add(mantenimiento);
                        }
                    }
                }
            }
            return mantenimientos;
        }

        public async Task<Mantenimiento> GetByIdAsync(int id)
        {
            Mantenimiento mantenimiento = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idMantenimiento, nombre, tlfno, cif, idPista, correo FROM Mantenimiento WHERE Id = @Id";
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
                                IdPista = await _idPista.GetByIdAsync(reader.GetInt32(4)),
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

                string query = "INSERT INTO Mantenimiento (idMantenimiento, nombre, tlfno, cif, idPista, correo) VALUES (@idMantenimiento, @nombre, @fecha, @tipo, @idPista, @correo)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idMantenimiento", mantenimiento.IdMantenimiento);
                    command.Parameters.AddWithValue("@nombre", mantenimiento.Nombre);
                    command.Parameters.AddWithValue("@tlfno", mantenimiento.Tlfno);
                    command.Parameters.AddWithValue("@cif", mantenimiento.Cif);
                    command.Parameters.AddWithValue("@idPista", mantenimiento.IdPista);
                    command.Parameters.AddWithValue("@correo", mantenimiento.Correo);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Mantenimiento mantenimiento)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Mantenimiento SET idMantenimiento = @idMantenimiento, nombre = @nombre, tlfno = @tlfno, cif = @cif, idPista =@idPista, correo = @correo WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idMantenimiento", mantenimiento.IdMantenimiento);
                    command.Parameters.AddWithValue("@nombre", mantenimiento.Nombre);
                    command.Parameters.AddWithValue("@tlfno", mantenimiento.Tlfno);
                    command.Parameters.AddWithValue("@cif", mantenimiento.Cif);
                    command.Parameters.AddWithValue("@idPista", mantenimiento.IdPista);
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

                string query = "DELETE FROM Mantenimiento WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


    }

}