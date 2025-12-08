namespace Models;

public class Usuario {

    public int IdUsuario  {get;set;}
    public string Nombre {get;set;} = "";
    public string Apellido  {get;set;} ="";
    public int Telefono  {get;set;} 
    public string Direccion  {get;set;} ="";
    public DateTime FechaNac  {get;set;}

    public Usuario(){}

    public Usuario(string nombre, string apellido, int telefono, string direccion, DateTime fechaNac) {
        Nombre = nombre;
        Apellido = apellido;
        Telefono = telefono;
        Direccion = direccion;
        FechaNac = fechaNac;
    }
}
