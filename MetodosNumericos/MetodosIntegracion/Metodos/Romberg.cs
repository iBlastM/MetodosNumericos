using System;
using System.Collections.Generic;
using MetodosInterpolacion;
using NCalc;

namespace MetodosIntegracion.Metodos
{
    public class Romberg(string expr) : MetodoIntegracion
    {
        Expression expresion = new Expression(expr);
        // Propiedad para acceder a la tabla de Romberg
        public List<List<double>> TablaRomberg { get; private set; } = new();



        /// <summary>
        /// Realiza la integración usando el método de Romberg.
        /// </summary>
        /// <param name="a">Límite inferior</param>
        /// <param name="b">Límite superior</param>
        /// <param name="nMax">Número máximo de niveles (filas) en la tabla</param>
        /// <returns>Valor estimado de la integral</returns>
        public (double resultado, double?[,] tabla) Integrar(double a, double b, double tol = 1e-10, int maxIter = 10)
        {
            double?[,] R = new double?[maxIter, maxIter];
            double h = b - a;

            R[0, 0] = 0.5 * h * (Evaluar(expresion, a) + Evaluar(expresion, b));

            for (int i = 1; i < maxIter; i++)
            {
                h /= 2;

                double sum = 0;
                int num = 1 << (i - 1);
                for (int k = 1; k <= num; k++)
                {
                    sum += Evaluar(expresion, a + (2 * k - 1) * h);
                }

                R[i, 0] = 0.5 * R[i - 1, 0] + h * sum;

                for (int j = 1; j <= i; j++)
                {
                    R[i, j] = R[i, j - 1] + (R[i, j - 1]!.Value - R[i - 1, j - 1]!.Value) / (Math.Pow(4, j) - 1);
                }

                if (i > 0 && Math.Abs(R[i, i].Value - R[i - 1, i - 1].Value) < tol)
                    return (R[i, i].Value, R);
            }

            return (R[maxIter - 1, maxIter - 1]!.Value, R);
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
}
