namespace Models;

public class Material {

    public int IdMaterial  {get;set;}
    public Pista IdPista {get;set;}
    public string Nombre  {get;set;} ="";
    public int Cantidad  {get;set;}
    public int Disponibilidad  {get;set;}
    public DateTime FechaActu  {get;set;}

    public Material(){}

    public Material(Pista idPista, string nombre, int cantidad, int disponibilidad, DateTime fechaActu) {
        IdPista = idPista;
        Nombre = nombre;
        Cantidad = cantidad;
        Disponibilidad = disponibilidad;
        FechaActu = fechaActu;
    }

}

