namespace AA1.DTOs
{
    public class PistaDto
    {
        public int IdPista { get; set; }
        public string Nombre { get; set; } = "";
        public string Tipo { get; set; } = "";
        public string Direccion { get; set; } = "";
        public bool Activa { get; set; }
        public int PrecioHora { get; set; }
    }

    public class CreatePistaDto
    {
        public string Nombre { get; set; } = "";
        public string Tipo { get; set; } = "";
        public string Direccion { get; set; } = "";
        public bool Activa { get; set; }
        public int PrecioHora { get; set; }
    }

    public class UpdatePistaDto
    {
        public string Nombre { get; set; } = "";
        public string Tipo { get; set; } = "";
        public string Direccion { get; set; } = "";
        public bool Activa { get; set; }
        public int PrecioHora { get; set; }
    }
}