namespace AA1.DTOs
{
    public class ReseñaDto
    {
        public int IdReseña  {get;set;}
        public int Valoracion {get;set;}
        public string Titulo  {get;set;} ="";
        public string Descripcion  {get;set;} ="";
        public DateTime Fecha {get;set;}
        public int IdReserva {get;set;}
    }

    public class CreateReseñaDto
    {
        public int Valoracion {get;set;}
        public string Titulo  {get;set;} ="";
        public string Descripcion  {get;set;} ="";
        public DateTime Fecha {get;set;}
        public int IdReserva {get;set;}
    }

    public class UpdateReseñaDto
    {
        public int Valoracion {get;set;}
        public string Titulo  {get;set;} ="";
        public string Descripcion  {get;set;} ="";
        public DateTime Fecha {get;set;}
        public int IdReserva {get;set;}
    }
}