namespace MetodosInterpolacion.Metodos;
public class DiferenciasDivididas(List<Punto> Puntos, int GradoInterpolacion, double xInterpolado)
{
    List<Punto> puntos = Puntos;
    public double[,] tablaDiferenciasDivididas { get; private set; } = new double[Puntos.Count + Puntos.Count - 1, 1 + Puntos.Count];
    public double[] coeficientes { get; private set; } = new double[Puntos.Count];
    public string polinomioInterpolacion { get; private set; }
    public List<Punto> puntosGraficar { get; private set; }

    public double CalcularInterpolacion() //Metodo principal
    {
        for (int k = 0; k < tablaDiferenciasDivididas.GetLength(0); k++)
        {
            for (int j = 0; j < tablaDiferenciasDivididas.GetLength(1); j++)
            {
                tablaDiferenciasDivididas[k, j] = -0.321123321123321123; // Asignar nulo en todas las celdas
            }
        }
        int i = 0;
        foreach (var punto in puntos)
        {
            tablaDiferenciasDivididas[i, 0] = punto.X;
            tablaDiferenciasDivididas[i, 1] = punto.FX;
            i += 2;
        }
        CalcularTabla();

        double resultadoInterpolacion = CalcularPuntoInterpolado(xInterpolado);

        polinomioInterpolacion = "P(x) = " + coeficientes[0].ToString();
        GenerarCadenaPolinomio();

        puntosGraficar = GenerarPuntos(xInterpolado, CalcularPuntoInterpolado);

        return resultadoInterpolacion;
    }

    private void CalcularTabla()
    {
        int j = 0; //Filas, empieza en la primera fila
        int k = 1; //Columnas, empieza en la segunda columna, osea las f(x)
        while (k < puntos.Count)
        {
            for (int i = 0; i < puntos.Count - k; i++)
            {
                tablaDiferenciasDivididas[j + 1, k + 1] = CalcularDiferenciaDividida(tablaDiferenciasDivididas[j, k], tablaDiferenciasDivididas[j + 2, k], j, k);
                j += 2;
            }
            k++;
            j = k - 1;
        }

        for (int i = 0; i < puntos.Count; i++)
        {
            coeficientes[i] = tablaDiferenciasDivididas[i, i + 1];
        }
    }
    private double CalcularDiferenciaDividida(double a, double b, int j, int k)
    {
        return (b - a) / (tablaDiferenciasDivididas[j + k + 1, 0] - tablaDiferenciasDivididas[j - k + 1, 0]);
    }

    private double CalcularPuntoInterpolado(double x)
    {
        double resultadoInterpolacion = coeficientes[0];

        for (int j = 1; j < GradoInterpolacion + 1; j++)
        {
            double producto = coeficientes[j];
            for (int k = 0; k < j; k++)
            {
                producto *= x - puntos[k].X;
            }
            resultadoInterpolacion += producto;
        }

        return resultadoInterpolacion;
    }

    private void GenerarCadenaPolinomio()
    {
        for (int i = 1; i < GradoInterpolacion + 1; i++)
        {
            polinomioInterpolacion += $" + ({coeficientes[i]})";
            for (int j = 0; j < i; j++)
            {
                polinomioInterpolacion += $"(x - {puntos[j].X})";
            }
        }
    }

    public List<Punto> GenerarPuntos(double xInicial, Func<double, double> calcularFX)
    {
        List<Punto> puntos = new List<Punto>();

        // Generar 500 puntos a la izquierda
        for (int i = 500; i > 0; i--)
        {
            double x = xInicial - (i * 0.1);
            puntos.Add(new Punto
            {
                X = x,
                FX = calcularFX(x) // Se calcula FX 
            });
        }

        // Generar el punto base (X inicial)
        puntos.Add(new Punto
        {
            X = xInicial,
            FX = calcularFX(xInicial)
        });

        // Generar 500 puntos a la derecha
        for (int i = 1; i <= 500; i++)
        {
            double x = xInicial + (i * 0.1);
            puntos.Add(new Punto
            {
                X = x,
                FX = calcularFX(x) // Se calcula FX con tu método
            });
        }

        return puntos;
    }
}
