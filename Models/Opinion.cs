namespace Models;

public class Opinion {

    public int IdOpinion  {get;set;}
    public string Nombre  {get;set;} ="";
    public string Texto  {get;set;} ="";
    public int Puntuacion  {get;set;}
    public DateTime FechaCrea  {get;set;}
    public Pista IdPista {get;set;}

    public Opinion(){}

   public Opinion(int idOpinion, string nombre, string texto, int puntuacion, DateTime fechaCrea) {
        IdOpinion = idOpinion;
        Nombre = nombre;
        Texto = texto;
        Puntuacion = puntuacion;
        FechaCrea = fechaCrea;
    }

}