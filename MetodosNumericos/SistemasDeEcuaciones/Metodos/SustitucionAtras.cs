namespace SistemasDeEcuaciones.Metodos;
public class SustitucionAtras(List<Termino> terminos) : IMetodo
{
    public List<Termino> Terminos = terminos;
    public List<Paso> pasos = new();

    public bool CalcularSoluciones(double[,] matriz)
    {
        return true;
    }
}
