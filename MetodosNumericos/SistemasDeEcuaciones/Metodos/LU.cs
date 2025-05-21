using System;
using System.Collections.Generic;

namespace SistemasDeEcuaciones.Metodos
{
    public class LU(List<Termino> terminos) : IMetodo
    {
        public List<Termino> Terminos = terminos;
        public List<PasoLU> pasos = new();

        public bool CalcularSoluciones(double[,] matriz)
        {
            int n = matriz.GetLength(0);
            double[,] A = new double[n, n];
            double[] b = new double[n];
            double[,] L = new double[n, n];
            double[,] U = new double[n, n];

            // Separar A y b de la matriz extendida
            for (int i = 0; i < n; i++)
            {
                b[i] = matriz[i, n];
                for (int j = 0; j < n; j++)
                    A[i, j] = matriz[i, j];
            }

            // Inicializar L y U
            for (int i = 0; i < n; i++)
                L[i, i] = 1;

            // Factorización LU
            for (int k = 0; k < n; k++)
            {
                for (int j = k; j < n; j++)
                {
                    double sum = 0;
                    for (int s = 0; s < k; s++)
                        sum += L[k, s] * U[s, j];
                    U[k, j] = A[k, j] - sum;
                }

                for (int i = k + 1; i < n; i++)
                {
                    double sum = 0;
                    for (int s = 0; s < k; s++)
                        sum += L[i, s] * U[s, k];
                    L[i, k] = (A[i, k] - sum) / U[k, k];
                }

                pasos.Add(new PasoLU
                {
                    operacion = $"Paso {k + 1}: Actualización de matrices L y U",
                    matrizL = (double[,])L.Clone(),
                    matrizU = (double[,])U.Clone()
                });
            }

            // Sustitución hacia adelante: Ly = b
            double[] y = new double[n];
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < i; j++)
                    sum += L[i, j] * y[j];
                y[i] = b[i] - sum;

                pasos.Add(new PasoLU
                {
                    operacion = $"Paso {pasos.Count + 1}: Sustitución hacia adelante: y[{i}] = {y[i]}",
                    matrizL = (double[,])L.Clone(),
                    matrizU = (double[,])U.Clone(),
                    vectorY = (double[])y.Clone()
                });
            }

            // Sustitución hacia atrás: Ux = y
            double[] x = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < n; j++)
                    sum += U[i, j] * x[j];
                x[i] = (y[i] - sum) / U[i, i];

                pasos.Add(new PasoLU
                {
                    operacion = $"Paso {pasos.Count + 1}: Sustitución hacia atrás: x[{i}] = {x[i]}",
                    matrizL = (double[,])L.Clone(),
                    matrizU = (double[,])U.Clone(),
                    vectorX = (double[])x.Clone(),
                    vectorY = (double[])y.Clone()
                });
            }

            for (int i = 0; i < n; i++)
                Terminos[i].Solucion = x[i];

            return true;
        }
    }
}
