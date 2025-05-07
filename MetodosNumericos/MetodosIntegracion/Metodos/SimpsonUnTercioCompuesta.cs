using MetodosInterpolacion;
using NCalc;

namespace MetodosIntegracion.Metodos;

public class SimpsonUnTercioCompuesta(string expr) : MetodoIntegracion
{
    Expression expresion = new Expression(expr);

    public double Integrar(double a, double b, int n)
    {
        if (n % 2 != 0)
            throw new ArgumentException("El número de subintervalos debe ser par para la regla de Simpson 1/3 compuesta.");

        double h = (b - a) / n;
        double suma = Evaluar(expresion, a) + Evaluar(expresion, b);

        for (int i = 1; i < n; i++)
        {
            double x = a + i * h;
            suma += (i % 2 == 0 ? 2 : 4) * Evaluar(expresion, x);
        }

        return (h / 3) * suma;
    }

    public List<Punto> obtenerPuntosEvaluados(double a, double b, int n)
    {
        List<Punto> puntos = new();
        double h = (b - a) / n;

        for (int i = 0; i <= n; i++)
        {
            double x = a + i * h;
            puntos.Add(new Punto
            {
                X = x,
                FX = Evaluar(expresion, x)
            });
        }

        return puntos;
    }

    public List<Punto> obtenerPuntosGraficarFuncionReal(double a, double b)
    {
        List<Punto> puntos = new();
        double paso = (b - a) / 500.0;

        for (int i = 0; i <= 500; i++)
        {
            double x = a + i * paso;
            puntos.Add(new Punto
            {
                X = x,
                FX = Evaluar(expresion, x)
            });
        }

        return puntos;
    }
}
