namespace MetodosInterpolacion.Metodos;

public class MetodoNeville(List<Punto> Puntos, int GradoInterpolacion, double xInterpolado)
{
    List<Punto> puntos = Puntos;
    public double[,] tablaNeville { get; private set; } = new double[Puntos.Count, Puntos.Count + 1];
    public string polinomioInterpolacion { get; private set; }
    public List<Punto> puntosGraficar { get; private set; }

    public double CalcularInterpolacion()
    {
        for (int k = 0; k < tablaNeville.GetLength(0); k++)
        {
            for (int j = 0; j < tablaNeville.GetLength(1); j++)
            {
                tablaNeville[k, j] = -0.321123321123321123; // Asignar nulo en todas las celdas
            }
        }
        // Inicializar la tabla con los valores de x y f(x)
        for (int i = 0; i < puntos.Count; i++)
        {
            tablaNeville[i, 0] = puntos[i].X;
            tablaNeville[i, 1] = puntos[i].FX;
        }

        // Llenar la tabla de Neville
        for (int j = 2; j <= GradoInterpolacion + 1; j++)
        {
            for (int i = 0; i < puntos.Count - (j - 1); i++)
            {
                tablaNeville[i, j] = ((xInterpolado - tablaNeville[i + (j - 1), 0]) * tablaNeville[i, j - 1] -
                                       (xInterpolado - tablaNeville[i, 0]) * tablaNeville[i + 1, j - 1]) /
                                      (tablaNeville[i, 0] - tablaNeville[i + (j - 1), 0]);
            }
        }

        double resultadoInterpolacion = tablaNeville[0, GradoInterpolacion + 1];
        Console.WriteLine($"El resultado en grado {GradoInterpolacion} es: {resultadoInterpolacion}");

        polinomioInterpolacion = $"Polinomio de Neville de grado {GradoInterpolacion}";

        puntosGraficar = GenerarPuntos(xInterpolado, CalcularPuntoInterpolado);
        return resultadoInterpolacion;
    }

    private double CalcularPuntoInterpolado(double x)
    {
        return tablaNeville[0, GradoInterpolacion + 1];
    }

    public List<Punto> GenerarPuntos(double xInicial, Func<double, double> calcularFX)
    {
        List<Punto> puntos = new();
        for (int i = 500; i > 0; i--)
        {
            double x = xInicial - (i * 0.1);
            puntos.Add(new Punto { X = x, FX = calcularFX(x) });
        }
        puntos.Add(new Punto { X = xInicial, FX = calcularFX(xInicial) });
        for (int i = 1; i <= 500; i++)
        {
            double x = xInicial + (i * 0.1);
            puntos.Add(new Punto { X = x, FX = calcularFX(x) });
        }
        return puntos;
    }
}
