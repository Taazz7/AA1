
using Microsoft.Data.SqlClient;
using Models;

namespace AA1.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly string _connectionString;
        private readonly int _idReserva;
        private readonly IUsuarioRepository _usuariorepository;
        private readonly IPistaRepository _pistarepository;
        private readonly DateTime _fecha;
        private readonly int _horas;
        private readonly int _precio;

        public ReservaRepository(IConfiguration configuration, int idReserva, IUsuarioRepository usuariorepository, IPistaRepository pistarepository, DateTime fecha, int horas, int precio)
        {
             _connectionString = configuration.GetConnectionString("AA1") ?? "Not found";
            _idReserva = idReserva;
            _usuariorepository = usuariorepository;
            _pistarepository = pistarepository;
            _fecha = fecha;
            _horas = horas;
            _precio = precio;
        }
        

        public async Task<List<Reserva>> GetAllAsync()
        {
            var reservas = new List<Reserva>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idReserva, idUsuario, idPista, fecha, horas, precio FROM RESERVAS";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var reserva = new Reserva
                            {
                                IdReserva = reader.GetInt32(0),
                                IdUsuario = await _usuariorepository.GetByIdAsync(reader.GetInt32(1)),
                                IdPista = await _pistarepository.GetByIdAsync(reader.GetInt32(2)),
                                Fecha = reader.GetDateTime(3),
                                Horas = reader.GetInt32(4),
                                Precio = reader.GetInt32(5)
                            }; 

                            reservas.Add(reserva);
                        }
                    }
                }
            }
            return reservas;
        }

        public async Task<Reserva> GetByIdAsync(int id)
        {
            Reserva reserva = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT idReserva, idUsuario, idPista, fecha, horas, precio FROM RESERVAS WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            reserva = new Reserva
                            {
                                IdReserva = reader.GetInt32(0),
                                IdUsuario = await _usuariorepository.GetByIdAsync(reader.GetInt32(1)),
                                IdPista = await _pistarepository.GetByIdAsync(reader.GetInt32(2)),
                                Fecha = reader.GetDateTime(3),
                                Horas = reader.GetInt32(4),
                                Precio = reader.GetInt32(5)
                            };
                        }
                    }
                }
            }
            return reserva;
        }

        public async Task AddAsync(Reserva reserva)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO RESERVAS (idReserva, idUsuario, idPista, fecha, horas, precio) VALUES (@idReserva, @idUsuario, @idPista, @fecha, @horas, @precio)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idReserva", reserva.IdReserva);
                    command.Parameters.AddWithValue("@idUsuario", reserva.IdUsuario);
                    command.Parameters.AddWithValue("@idPista", reserva.IdPista);
                    command.Parameters.AddWithValue("@fecha", reserva.Fecha);
                    command.Parameters.AddWithValue("@horas", reserva.Horas);
                    command.Parameters.AddWithValue("@precio", reserva.Precio);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Reserva reserva)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE RESERVAS SET idReserva = @idReserva, idUsuario = @idUsuario, idPista = @idPista, fecha = @fecha, horas =@horas, precio = @precio WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idReserva", reserva.IdReserva);
                    command.Parameters.AddWithValue("@idUsuario", reserva.IdUsuario);
                    command.Parameters.AddWithValue("@idPista", reserva.IdPista);
                    command.Parameters.AddWithValue("@fecha", reserva.Fecha);
                    command.Parameters.AddWithValue("@horas", reserva.Horas);
                    command.Parameters.AddWithValue("@precio", reserva.Precio);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM RESERVAS WHERE Id = @Id";
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
                    INSERT INTO RESERVAS (idReserva, idUsuario, idPista, fecha, horas, precio)
                    VALUES 
                    (@idReserva1, @idUsuario1, @idPista1, @fecha1, @horas1, @precio1),
                    (@idReserva2, @idUsuario2, @idPista2, @fecha2, @horas2, @precio2)";

                using (var command = new SqlCommand(query, connection))
                {
                    // Parámetros para el primer bebida
                    command.Parameters.AddWithValue("@idReserva1", 1);
                    command.Parameters.AddWithValue("@idUsuario1", 2);
                    command.Parameters.AddWithValue("@idPista1", 1);
                    command.Parameters.AddWithValue("@fecha1", "2025-11-20");
                    command.Parameters.AddWithValue("@horas1", 1);
                    command.Parameters.AddWithValue("@precio1", 20);
                    

                    // Parámetros para el segundo bebida
                    command.Parameters.AddWithValue("@idReserva2", 2);
                    command.Parameters.AddWithValue("@idUsuario2", 1);
                    command.Parameters.AddWithValue("@idPista2", 2);
                    command.Parameters.AddWithValue("@fecha2", "2025-11-21");
                    command.Parameters.AddWithValue("@horas2", 3);
                    command.Parameters.AddWithValue("@precio2", 28);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


    }

}