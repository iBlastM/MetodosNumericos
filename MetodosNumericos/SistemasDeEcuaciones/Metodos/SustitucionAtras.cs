namespace SistemasDeEcuaciones.Metodos;
public class SustitucionAtras(List<Termino> terminos) : IMetodo
{
    public List<Termino> Terminos = terminos;
    public List<Paso> pasos = new();

    public bool CalcularSoluciones(double[,] matriz)
    {
        int n = matriz.GetLength(0);

        //Paso 2
        int p = 0;
        int fila = 0;

        while (matriz[p, fila] == 0 && p <= n - 1) p++;
        if (matriz[p, fila] == 0) return false;

        //Paso 3
        if (p != fila)
        { // Intercambiar filas
            for (int j = 0; j <= n; j++)
            {
                double temp = matriz[fila, j];
                matriz[fila, j] = matriz[p, j];
                matriz[p, j] = temp;
            }
            pasos.Add(new Paso
            {
                operacion = $"Intercambiar fila {fila + 1} con fila {p + 1}",
                matriz = matriz.Clone() as double[,]
            });
        }

        // Paso 1
        for (int i = 0; i < n - 1; i++)
        {
            

            //Paso 4
            for(int j = i + 1; j < n; j++)
            {
                //Paso 5
                double factor = matriz[j, i] / matriz[i, i];

                //Paso 6
                for (int k = i; k < n + 1; k++)
                {
                    matriz[j, k] -= factor * matriz[i, k];
                }
                pasos.Add(new Paso
                {
                    operacion = $"Fila {j + 1} - ({factor}) * Fila {i + 1}",
                    matriz = matriz.Clone() as double[,]
                });
            }
        }

        //Paso 7
        if (matriz[n - 1, n - 1] == 0) return false;
        
        //Paso 8
        Terminos[^1].Solucion = matriz[n - 1, n] / matriz[n - 1, n - 1];
        pasos.Add(new Paso
        {
            operacion = $"Solución de la última variable ({Terminos[^1].Variable}): matriz[n - 1, n] / matriz[n - 1, n - 1] = {matriz[n - 1, n] / matriz[n - 1, n - 1]}",
            matriz = matriz.Clone() as double[,]
        });

        //Paso 9
        for (int i = n - 2; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i + 1; j < n; j++)
            {
                sum += matriz[i, j] * Terminos[j].Solucion;
            }
            Terminos[i].Solucion = (matriz[i, n] - sum) / matriz[i, i];
            pasos.Add(new Paso
            {
                operacion = $"Solución de la variable {Terminos[i].Variable}: (matriz[{i}, n] - ({sum})) / matriz[{i}, {i}] = {(matriz[i, n] - sum) / matriz[i, i]}",
                matriz = matriz.Clone() as double[,]
            });
        }

        //Paso 10
        return true;
    }
}
