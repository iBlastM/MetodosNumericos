using MetodosInterpolacion;
using MetodosInterpolacion.Metodos;
using NCalc;

namespace MetodosIntegracion.Metodos;
public class SimpsonTresOctavosSimple(string expr) : MetodoIntegracion
{

    Expression expresion = new Expression(expr);
    public double Integrar(double a, double b)
    {
        double h38 = (b - a) / 3;
        double resultado = 3 * h38 / 8 * (Evaluar(expresion, a) + 3 * Evaluar(expresion, a + h38) + 3 *
            Evaluar(expresion, a + 2 * h38) + Evaluar(expresion, b));
        return resultado;
    }

    public List<Punto> obtenerPuntosGraficar(double a, double b)
    {
        double h38 = (b - a) / 3;
        List<Punto> puntosSimpson = new()
        {
            new Punto { X = a, FX = Evaluar(expresion, a) },
            new Punto { X = a + h38, FX = Evaluar(expresion, a + h38) },
            new Punto { X = a + 2 * h38, FX = Evaluar(expresion, a + 2 * h38) },
            new Punto { X = b, FX = Evaluar(expresion, b) }
        };

        double xParaEvaluar = a + (b - a) / 2;
        double distanciaEntrePuntos = (b - a) / 2;

        var lagrange = new InterpolacionLagrange(puntosSimpson, 3, xParaEvaluar, distanciaEntrePuntos);
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
