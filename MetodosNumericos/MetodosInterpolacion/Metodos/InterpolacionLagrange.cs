namespace MetodosInterpolacion.Metodos;
public class InterpolacionLagrange(List<Punto> Puntos, int gradoInterpolacion, double xInterpolado, double distanciaPuntosGenerados = 0)
{
    List<Punto> puntos = Puntos;
    public double[,] tablaDiferenciasDivididas { get; private set; } = new double[Puntos.Count + Puntos.Count - 1, 1 + Puntos.Count];
    public double[] coeficientes { get; private set; } = new double[Puntos.Count];
    public string polinomioInterpolacion { get; private set; }
    public List<Punto> puntosGraficar { get; private set; }

    public bool poqutiosPuntos { get;  set; } = false;
    public double CalcularInterpolacion()
    {
        polinomioInterpolacion = $"Polinomio en grado {gradoInterpolacion}: P(x) = ";
        GenerarCadenaPolinomio();
        double resultadoInterpolacion = CalcularPuntoInterpolado(xInterpolado);
        if(poqutiosPuntos) puntosGraficar = GenerarPoquitosPuntos(xInterpolado, CalcularPuntoInterpolado);
        else puntosGraficar = GenerarPuntos(xInterpolado, CalcularPuntoInterpolado);
        return resultadoInterpolacion;
    }

    private double CalcularPuntoInterpolado(double x)
    {
        double result = 0;
        double aux = 1;
        for (int k = 0; k < gradoInterpolacion + 1; k++)
        {
            for (int i = 0; i < gradoInterpolacion + 1; i++)
            {
                if (i != k)
                {
                    aux *= (x - puntos[i].X) / (puntos[k].X - puntos[i].X);
                }

            }
            aux *= puntos[k].FX;
            result += aux;
            aux = 1;
        }
        return result;
    }

    private void GenerarCadenaPolinomio()
    {
        for (int k = 0; k < gradoInterpolacion + 1; k++)
        {
            for (int i = 0; i < gradoInterpolacion + 1; i++)
            {
                if (i != k)
                {
                    polinomioInterpolacion += $"( x - {puntos[i].X} )";
                }

            }
            polinomioInterpolacion += "/";
            for (int i = 0; i < gradoInterpolacion + 1; i++)
            {
                if (i != k)
                {
                    polinomioInterpolacion += $"( {puntos[k].X} - {puntos[i].X} )";
                }

            }
            if (k == gradoInterpolacion)
            {
                polinomioInterpolacion += $"{puntos[k].FX}";
            }
            else
            {
                polinomioInterpolacion += $"{puntos[k].FX} + ";
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

    public List<Punto> GenerarPoquitosPuntos(double xInicial, Func<double, double> calcularFX)
    {
        List<Punto> puntos = new List<Punto>();

        distanciaPuntosGenerados = distanciaPuntosGenerados / 500;

        // Generar 500 puntos a la izquierda
        for (int i = 500; i > 0; i--)
        {
            double x = xInicial - (i * distanciaPuntosGenerados);
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
            double x = xInicial + (i * distanciaPuntosGenerados);
            puntos.Add(new Punto
            {
                X = x,
                FX = calcularFX(x) // Se calcula FX con tu método
            });
        }

        return puntos;
    }

}
