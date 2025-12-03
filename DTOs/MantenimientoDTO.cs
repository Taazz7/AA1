namespace AA1.DTOs
{
    public class MantenimientoDto
    {
        public int IdMantenimiento { get; set; }
        public string Nombre { get; set; } = "";
        public int Tlfno { get; set; }
        public int Cif { get; set; }
        public int IdPista { get; set; }
        public string Correo { get; set; } = "";
    }

    public class CreateMantenimientoDto
    {
        public string Nombre { get; set; } = "";
        public int Tlfno { get; set; }
        public int Cif { get; set; }
        public int IdPista { get; set; }
        public string Correo { get; set; } = "";
    }

    public class UpdateMantenimientoDto
    {
        public string Nombre { get; set; } = "";
        public int Tlfno { get; set; }
        public int Cif { get; set; }
        public int IdPista { get; set; }
        public string Correo { get; set; } = "";
    }
}