using MetodosInterpolacion;
using MetodosInterpolacion.Metodos;
using NCalc;

namespace MetodosIntegracion.Metodos;
public class SimpsonUnTercioSimple(string expr) : MetodoIntegracion
{
    Expression expresion = new Expression(expr);

    public double Integrar(double a, double b)
    {
        double h = (b - a) / 2;
        double resultado = h / 3 * (Evaluar(expresion, a) + 4 * Evaluar(expresion, a + h) + Evaluar(expresion, b));
        return resultado;
    }

    public List<Punto> obtenerPuntosGraficar(double a, double b)
    {
        double h = (b - a) / 2;
        List<Punto> puntosSimpson = new()
        {
            new Punto { X = a, FX = Evaluar(expresion, a) },
            new Punto { X = a + h, FX = Evaluar(expresion, a + h) },
            new Punto { X = b, FX = Evaluar(expresion, b) }
        };

        double xParaEvaluar = a + h;
        double distanciaEntrePuntos = h;

        var lagrange = new InterpolacionLagrange(puntosSimpson, 2, xParaEvaluar, distanciaEntrePuntos);
        lagrange.poqutiosPuntos = true;
        double resultado = lagrange.CalcularInterpolacion();
        List<Punto> puntosGrafica = lagrange.puntosGraficar;
        return puntosGrafica;
    }

    public List<Punto> obtenerPuntosGraficarFuncionReal(double a, double b)
    {
        List<Punto> puntos = new List<Punto>();
        double xIntermedio = a + (b - a) / 2;
        double distanciaEntrePuntos = (b - a) / 2 / 500;

        for (int i = 500; i > 0; i--)
        {
            double x = xIntermedio - (i * distanciaEntrePuntos);
            puntos.Add(new Punto { X = x, FX = Evaluar(expresion, x) });
        }

        puntos.Add(new Punto { X = xIntermedio, FX = Evaluar(expresion, xIntermedio) });

        for (int i = 1; i <= 500; i++)
        {
            double x = xIntermedio + (i * distanciaEntrePuntos);
            puntos.Add(new Punto { X = x, FX = Evaluar(expresion, x) });
        }

        return puntos;
    }
}
