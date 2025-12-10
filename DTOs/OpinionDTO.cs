namespace AA1.DTOs
{
    public class OpinionDto
    {
        public int IdOpinion { get; set; }
        public string Nombre { get; set; }="";
        public string Texto { get; set; }="";
        public int Puntuacion { get; set; }
        public DateTime FechaCrea { get; set; }
        public int IdPista { get; set; }

    }

    public class CreateOpinionDto
    {
        public int IdOpinion { get; set; }
        public string Nombre { get; set; }="";
        public string Texto { get; set; }="";
        public int Puntuacion { get; set; }
        public DateTime FechaCrea { get; set; }
        public int IdPista { get; set; }

    }

    public class UpdateOpinionDto
    {
        public string Nombre { get; set; }="";
        public string Texto { get; set; }="";
        public int Puntuacion { get; set; }
        public DateTime FechaCrea { get; set; }
        public int IdPista { get; set; }
    }
}