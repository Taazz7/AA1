namespace Models;

public class Pista {

    public int IdPista  {get;set;}
    public string Tipo {get;set;} = "";
    public string Nombre  {get;set;} ="";
    public string Direccion  {get;set;} ="";
    public bool Activa  {get;set;}
    public int PrecioHora  {get;set;}

    public Pista(){}

    public Pista(string tipo, string nombre, string direccion, bool activa, int precioHora) {
        Tipo = tipo;
        Nombre = nombre;
        Direccion = direccion;
        Activa = activa;
        PrecioHora = precioHora;
    }





}
