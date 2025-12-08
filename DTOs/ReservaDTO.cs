namespace AA1.DTOs
{
    public class ReservaDto
    {
        public int IdReserva { get; set; }
        public int IdUsuario { get; set; }
        public int IdPista { get; set; }
        public DateTime Fecha { get; set; }
        public int Horas { get; set; }
        public int Precio { get; set; }
    }

    public class CreateReservaDto
    {
        public int IdReserva { get; set; }
        public int IdUsuario { get; set; }
        public int IdPista { get; set; }
        public DateTime Fecha { get; set; }
        public int Horas { get; set; }
        public int Precio { get; set; }
    }

    public class UpdateReservaDto
    {
        public int IdUsuario { get; set; }
        public int IdPista { get; set; }
        public DateTime Fecha { get; set; }
        public int Horas { get; set; }
        public int Precio { get; set; }
    }
}