
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
        private readonly IPistaRepository _idPista;
        private readonly int _cif;
        private readonly string _correo;

        public MantenimientoRepository(IConfiguration configuration, int idMantenimiento, string nombre, int tlfno, IPistaRepository idPista, int cif, string correo)
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


        public async Task InicializarDatosAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Comando SQL para insertar datos iniciales
                var query = @"
                    INSERT INTO Mantenimiento (idMantenimiento, nombre, tlfno, cif, idPista, correo)
                    VALUES 
                    (@idMantenimiento1, @nombre1, @tlfno1, @cif1, @idPista1, @correo1),
                    (@idMantenimiento2, @nombre2, @tlfno2, @cif2, @idPista2, @correo2)";

                using (var command = new SqlCommand(query, connection))
                {
                    // Parámetros para el primer bebida
                    command.Parameters.AddWithValue("@idMantenimiento1", 1);
                    command.Parameters.AddWithValue("@nombre1", "Revisión red");
                    command.Parameters.AddWithValue("@tlfno1", 152847563);
                    command.Parameters.AddWithValue("@cif1", 258);
                    command.Parameters.AddWithValue("@idPista1", 1);
                    command.Parameters.AddWithValue("@correo1", "mantenimiento@club.com");
                    

                    // Parámetros para el segundo bebida
                    command.Parameters.AddWithValue("@idMantenimiento2", 2);
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