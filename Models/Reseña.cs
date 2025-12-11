namespace Models;

public class Reseña {

    public int IdReseña  {get;set;}
    public Reserva? IdReserva {get;set;}
    public int Valoracion {get;set;}
    public string Titulo  {get;set;} ="";
    public string Descripcion  {get;set;} ="";
    public DateTime Fecha {get;set;}
    public Reseña(){}

    public Reseña(int valoracion, string titulo, string descripcion) {
        Valoracion = valoracion;
        Titulo = titulo;
        Descripcion = descripcion;
    }





}
