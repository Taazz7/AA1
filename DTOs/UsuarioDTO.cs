namespace AA1.DTOs
{
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public int Telefono { get; set; }
        public string Direccion { get; set; } = "";
        public DateTime FechaNac { get; set; }
    }

    public class CreateUsuarioDto
    {
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public int Telefono { get; set; }
        public string Direccion { get; set; } = "";
        public DateTime FechaNac { get; set; }
    }

    public class UpdateUsuarioDto
    {
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public int Telefono { get; set; }
        public string Direccion { get; set; } = "";
        public DateTime FechaNac { get; set; }
    }
}