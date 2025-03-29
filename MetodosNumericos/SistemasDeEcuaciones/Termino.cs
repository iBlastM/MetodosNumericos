namespace SistemasDeEcuaciones;
public class Termino
{
    public string Variable { get; set; }
    public double[] Coeficientes { get; set; }
    public double Solucion { get; set; }

    public Termino(int n, string variable)
    {
        Variable = variable;
        Coeficientes = new double[n];
    }
}
