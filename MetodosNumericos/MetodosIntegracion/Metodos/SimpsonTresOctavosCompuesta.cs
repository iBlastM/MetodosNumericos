using MetodosInterpolacion.Metodos;
using MetodosInterpolacion;
using NCalc;

namespace MetodosIntegracion.Metodos;
public class SimpsonTresOctavosCompuesta(string expr) : MetodoIntegracion
{
    Expression expresion = new Expression(expr);
    public double Integrar(double a, double b, int n)
    {
        double h383 = (b - a) / n;
        double sum383 = 0;
        for (int i = 1; i < n; i++)
        {
            int coef = (i % 3 == 0) ? 2 : 3;
            sum383 += coef * Evaluar(expresion, a + i * h383);
        }
        double resultado = 3 * h383 / 8 * (Evaluar(expresion, a) + sum383 + Evaluar(expresion, b));

        return resultado;
    }

    public List<Punto> obtenerPuntosGraficar(double a, double b, int n)
    {
        var puntos = new List<Punto>();
        double h = (b - a) / n;

        for (int i = 0; i <= n; i++)
        {
            double xi = a + i * h;
            double fxi = Evaluar(expresion, xi);
            puntos.Add(new Punto
            {
                X = xi,
                FX = fxi
            });
        }

        double xParaEvaluar = a + (b - a) / 2;
        double distanciaEntrePuntos = (b - a) / 2;

        var lagrange = new InterpolacionLagrange(puntos, puntos.Count - 1, xParaEvaluar, distanciaEntrePuntos);
        lagrange.poqutiosPuntos = true;
        double resultado = lagrange.CalcularInterpolacion();
        List<Punto> puntosGrafica = lagrange.puntosGraficar;
        return puntosGrafica;
    }

    public List<Punto> obtenerPuntosGraficarFuncionReal(double a, double b)
    {
        List<Punto> puntos = new List<Punto>();

        double xIntermedio = a + (b - a) / 2;
        double distanciaEntrePuntos = (b - a) / 2;

        distanciaEntrePuntos = distanciaEntrePuntos / 500;

        // Generar 500 puntos a la izquierda
        for (int i = 500; i > 0; i--)
        {
            double x = xIntermedio - (i * distanciaEntrePuntos);
            puntos.Add(new Punto
            {
                X = x,
                FX = Evaluar(expresion, x) // Se calcula FX 
            });
        }

        // Generar el punto base (X inicial)
        puntos.Add(new Punto
        {
            X = xIntermedio,
            FX = Evaluar(expresion, xIntermedio)
        });

        // Generar 500 puntos a la derecha
        for (int i = 1; i <= 500; i++)
        {
            double x = xIntermedio + (i * distanciaEntrePuntos);
            puntos.Add(new Punto
            {
                X = x,
                FX = Evaluar(expresion, x) // Se calcula FX con tu método
            });
        }

        return puntos;
    }
}
