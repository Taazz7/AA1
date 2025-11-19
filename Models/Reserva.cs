namespace Models;

public abstract class Reserva {

    public int IdReserva  {get;set;}
    public Usuario IdUsuario {get;set;}
    public Pista IdPista  {get;set;}
    public DateTime Fecha  {get;set;}
    public int Horas  {get;set;}
    public double Precio  {get;set;}

    public Reserva(){}

    public Reserva(Usuario idUsuario, Pista idPista, DateTime fecha, int horas, double precio) {
        IdUsuario = idUsuario;
        IdPista = idPista;
        Fecha = fecha;
        Horas = horas;
        Precio = precio;
    }

    public abstract void MostrarDetalles();

   // public abstract string MostrarDetallesGuardado();

}
