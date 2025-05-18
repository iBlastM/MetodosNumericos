using MetodosInterpolacion.Metodos;
using MetodosInterpolacion;
using NCalc;
using System;
using System.Runtime.Intrinsics.Arm;

namespace MetodosIntegracion.Metodos;

public class IntegracionGaussiana(string expr) : MetodoIntegracion
{
    private Expression expresion = new(expr);

    public List<Punto> puntos = new List<Punto>();

    public double Integrar(double a, double b, int n)
    {
        var (x, w) = CalcularNodosYPesos(n);
        double resultado = 0.0;

        for (int i = 0; i < n; i++)
        {
            // Cambio de variable: x' = (b - a)/2 * xi + (a + b)/2
            double xiTransformado = ((b - a) / 2.0) * x[i] + (a + b) / 2.0;
            puntos.Add(new Punto
            {
                X = xiTransformado,
                FX = Evaluar(expresion, xiTransformado)
            });
            resultado += w[i] * Evaluar(expresion, xiTransformado);
        }

        return ((b - a) / 2.0) * resultado;
    }

    private (double[] x, double[] w) CalcularNodosYPesos(int n)
    {
        var x = new double[n];
        var w = new double[n];

        for (int i = 0; i < n; i++)
        {
            double xi = Math.Cos(Math.PI * (i + 0.75) / (n + 0.5));
            double xiAnterior;

            //Newton-Raphson para encontrar los nodos (Raiz de los polinomios de Legendre)
            do
            {
                xiAnterior = xi;
                var (dp, p1) = LegendreDerivada(n, xi);
                xi = xiAnterior - p1 / dp;

            } while (Math.Abs(xi - xiAnterior) > 1e-14);

            x[i] = xi;
            var (xdp, _) = LegendreDerivada(n, xi);
            w[i] = 2.0 / ((1 - xi * xi) * Math.Pow(xdp, 2));
        }

        return (x, w);
    }

    private (double resultado, double p1) LegendreDerivada(int n, double x)
    {
        double p0 = 1.0;
        double p1 = x;
        for (int i = 2; i <= n; i++)
        {
            double p2 = ((2.0 * i - 1.0) * x * p1 - (i - 1.0) * p0) / i;
            p0 = p1;
            p1 = p2;
        }

        return (n * (x * p1 - p0) / (x * x - 1), p1);
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
