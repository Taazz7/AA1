namespace Models;

public abstract class Usuario {

    public int IdUsuario  {get;set;}
    public string Nombre {get;set;} = "";
    public string Apellidos  {get;set;} ="";
    public int Tlfno  {get;set;} 
    public string Direccion  {get;set;} ="";
    public DateTime FechaNac  {get;set;}

    public Usuario(){}

    public Usuario(string nombre, string apellidos, int tlfno, string direccion, DateTime fechaNac) {
        Nombre = nombre;
        Apellidos = apellidos;
        Tlfno = tlfno;
        Direccion = direccion;
        FechaNac = fechaNac;
    }

    public abstract void MostrarDetalles();

   // public abstract string MostrarDetallesGuardado();

}
