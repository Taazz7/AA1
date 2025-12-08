namespace AA1.DTOs
{
    public class MaterialDto
    {
        public int IdMaterial { get; set; }
        public string Nombre { get; set; } = "";
        public int Cantidad { get; set; }
        public int Disponibilidad { get; set; }
        public int IdPista { get; set; }
        public DateTime FechaActu { get; set; }
    }

    public class CreateMaterialDto
    {
        public string Nombre { get; set; } = "";
        public int Cantidad { get; set; }
        public int Disponibilidad { get; set; }
        public int IdPista { get; set; }
        public DateTime FechaActu { get; set; }
    }

    public class UpdateMaterialDto
    {
        public string Nombre { get; set; } = "";
        public int Cantidad { get; set; }
        public int Disponibilidad { get; set; }
        public int IdPista { get; set; }
        public DateTime FechaActu { get; set; }
    }
}