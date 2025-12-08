namespace Models;

public class Mantenimiento {

    public int IdMantenimiento  {get;set;}
    public string Nombre {get;set;} = "";
    public int Tlfno  {get;set;}
    public Pista? IdPista  {get;set;}
    public int Cif  {get;set;}
    public string Correo  {get;set;} ="";

    public Mantenimiento(){}

    public Mantenimiento(string nombre, int tlfno, Pista idPista, int cif, string correo) {
        Nombre = nombre;
        Tlfno = tlfno;
        IdPista = idPista;
        Cif = cif;
        Correo = correo;
    }


}
