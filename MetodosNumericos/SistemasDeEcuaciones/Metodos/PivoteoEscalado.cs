namespace SistemasDeEcuaciones.Metodos;

public class PivoteoEscalado(List<Termino> terminos) : IMetodo
{
    public List<Termino> Terminos = terminos;
    public List<Paso> pasos = new();

    public bool CalcularSoluciones(double[,] matriz)
    {
        int n = matriz.GetLength(0);
        int[] indice = Enumerable.Range(0, n).ToArray();
        double[] escalas = new double[n];

        // Determinar los valores de escala
        for (int i = 0; i < n; i++)
        {
            escalas[i] = matriz.Cast<double>().Skip(i * (n + 1)).Take(n).Max(Math.Abs);
            if (escalas[i] == 0) return false; // Matriz singular
        }

        for (int i = 0; i < n - 1; i++)
        {
            // Pivoteo escalado: encontrar la mejor fila
            int p = i;
            double maxRatio = 0;
            for (int j = i; j < n; j++)
            {
                double ratio = Math.Abs(matriz[indice[j], i]) / escalas[indice[j]];
                if (ratio > maxRatio)
                {
                    maxRatio = ratio;
                    p = j;
                }
            }

            // Intercambiar índices
            (indice[i], indice[p]) = (indice[p], indice[i]);
            pasos.Add(new Paso { operacion = $"Intercambiar fila {i + 1} con fila {p + 1}", matriz = (double[,])matriz.Clone() });

            // Eliminación
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

         
        if (matriz[indice[n - 1], n - 1] == 0) return false;
        Terminos[^1].Solucion = matriz[indice[n - 1], n] / matriz[indice[n - 1], n - 1];
        pasos.Add(new Paso { operacion = $"Solución variable {Terminos[^1].Variable}: {Terminos[^1].Solucion}", matriz = (double[,])matriz.Clone() });

        for (int i = n - 2; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i + 1; j < n; j++)
            {
                sum += matriz[indice[i], j] * Terminos[j].Solucion;
            }
            Terminos[i].Solucion = (matriz[indice[i], n] - sum) / matriz[indice[i], i];
            pasos.Add(new Paso { operacion = $"Solución variable {Terminos[i].Variable}: {Terminos[i].Solucion}", matriz = (double[,])matriz.Clone() });
        }
        return true;
    }
}
