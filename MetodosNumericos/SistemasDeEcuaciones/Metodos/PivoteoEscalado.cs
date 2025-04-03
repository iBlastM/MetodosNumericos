namespace SistemasDeEcuaciones.Metodos;
public class PivoteoEscalado(List<Termino> terminos) : IMetodo
{
    public List<Termino> Terminos = terminos;
    public List<Paso> pasos = new();

    public bool CalcularSoluciones(double[,] matriz)
    {
        int n = matriz.GetLength(0);
        int[] indice = Enumerable.Range(0, n).ToArray();
        double[] escala = new double[n];

        // Calcular el vector de escalas
        for (int i = 0; i < n; i++)
        {
            escala[i] = matriz.Cast<double>().Skip(i * (n + 1)).Take(n).Max(Math.Abs);
            if (escala[i] == 0) return false; // Matriz singular
        }

        for (int i = 0; i < n - 1; i++)
        {
            // Seleccionar pivote con criterio escalado
            int p = i;
            double maxRatio = 0;
            for (int j = i; j < n; j++)
            {
                double ratio = Math.Abs(matriz[indice[j], i]) / escala[indice[j]];
                if (ratio > maxRatio)
                {
                    maxRatio = ratio;
                    p = j;
                }
            }

            if (maxRatio == 0) return false; // Matriz singular

            // Intercambiar filas si es necesario
            if (p != i)
            {
                (indice[i], indice[p]) = (indice[p], indice[i]);
                pasos.Add(new Paso { operacion = $"Intercambiar fila {i + 1} con fila {p + 1} porque el pivote elegido ({matriz[indice[p], i]}) tiene el mayor ratio respecto a su escala.", matriz = (double[,])matriz.Clone() });
            }
            else
            {
                pasos.Add(new Paso { operacion = $"El pivote en la fila {i + 1} ({matriz[indice[i], i]}) ya es el mejor según la escala, no se intercambia.", matriz = (double[,])matriz.Clone() });
            }

            // Eliminación hacia adelante
            for (int j = i + 1; j < n; j++)
            {
                double factor = matriz[indice[j], i] / matriz[indice[i], i];
                for (int k = i; k < n + 1; k++)
                {
                    matriz[indice[j], k] -= factor * matriz[indice[i], k];
                }
                pasos.Add(new Paso { operacion = $"Fila {j + 1} - ({factor}) * Fila {i + 1} para eliminar el coeficiente en la columna {i + 1}.", matriz = (double[,])matriz.Clone() });
            }
        }

        if (matriz[indice[n - 1], n - 1] == 0) return false;

        // Resolver última variable
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
