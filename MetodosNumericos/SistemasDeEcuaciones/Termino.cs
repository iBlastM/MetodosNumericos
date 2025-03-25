namespace SistemasDeEcuaciones;
internal class Termino
{
    public string Variable { get; set; }
    public double[] Coeficientes { get; set; }

    public Termino(int n, string variable)
    {
        Variable = variable;
        Coeficientes = new double[n];
    }
}
