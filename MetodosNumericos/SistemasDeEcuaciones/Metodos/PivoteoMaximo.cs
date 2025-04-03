namespace SistemasDeEcuaciones.Metodos
{
    public class PivoteoMaximo(List<Termino> terminos) : IMetodo
    {
        public List<Termino> Terminos = terminos;
        public List<Paso> pasos = new();

        public bool CalcularSoluciones(double[,] matriz)
        {
            int n = matriz.GetLength(0);
            int[] indice = Enumerable.Range(0, n).ToArray();

            for (int i = 0; i < n - 1; i++)
            {
                // Pivoteo máximo de columnas: encontrar el máximo en la columna
                int p = i;
                double maxVal = 0;
                for (int j = i; j < n; j++)
                {
                    if (Math.Abs(matriz[indice[j], i]) > maxVal)
                    {
                        maxVal = Math.Abs(matriz[indice[j], i]);
                        p = j;
                    }
                }

                // Si no encontramos un pivote distinto de cero, la matriz es singular
                if (maxVal == 0) return false;

                // Verificamos si ya está en su lugar (sin necesidad de intercambio)
                if (p == i)
                {
                    pasos.Add(new Paso { operacion = $"No se realizó intercambio de filas en el paso {i + 1} porque las filas ya están en su lugar.", matriz = (double[,])matriz.Clone() });
                }
                else
                {
                    // Si hubo intercambio de filas
                    (indice[i], indice[p]) = (indice[p], indice[i]);
                    pasos.Add(new Paso { operacion = $"Intercambiar fila {i + 1} con fila {p + 1}", matriz = (double[,])matriz.Clone() });
                }

                // Eliminación hacia adelante
                for (int j = i + 1; j < n; j++)
                {
                    double factor = matriz[indice[j], i] / matriz[indice[i], i];
                    for (int k = i; k < n + 1; k++)
                    {
                        matriz[indice[j], k] -= factor * matriz[indice[i], k];
                    }
                    pasos.Add(new Paso { operacion = $"Fila {j + 1} - ({factor}) * Fila {i + 1}", matriz = (double[,])matriz.Clone() });
                }
            }

            // Comprobamos si la última fila es válida
            if (matriz[indice[n - 1], n - 1] == 0) return false;

            // Resolver la última variable
            Terminos[^1].Solucion = matriz[indice[n - 1], n] / matriz[indice[n - 1], n - 1];
            pasos.Add(new Paso { operacion = $"Solución de la última variable ({Terminos[^1].Variable}): {matriz[indice[n - 1], n]} / {matriz[indice[n - 1], n - 1]} = {Terminos[^1].Solucion}", matriz = (double[,])matriz.Clone() });

            // Sustitución hacia atrás
            for (int i = n - 2; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += matriz[indice[i], j] * Terminos[j].Solucion;
                }
                Terminos[i].Solucion = (matriz[indice[i], n] - sum) / matriz[indice[i], i];
                pasos.Add(new Paso { operacion = $"Solución de la variable {Terminos[i].Variable}: ({matriz[indice[i], n]} - ({sum})) / {matriz[indice[i], i]} = {Terminos[i].Solucion}", matriz = (double[,])matriz.Clone() });
            }

            return true;
        }
    }
}
