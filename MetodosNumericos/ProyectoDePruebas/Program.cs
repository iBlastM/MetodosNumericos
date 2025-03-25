using SistemasDeEcuaciones;

Console.WriteLine("x + y = 2, y - x = 0, 3x + y = 4");
double[,] matriz = Helpers.ObtenerMatrizAmpliada("x + y = 2, y - x = 0, 3x + y = 4");

static void ImprimirMatriz(double[,] matriz)
{
    int filas = matriz.GetLength(0);
    int columnas = matriz.GetLength(1);

    for (int i = 0; i < filas; i++)
    {
        for (int j = 0; j < columnas; j++)
        {
            Console.Write(matriz[i, j] + "\t");
        }
        Console.WriteLine(); 
    }
}

ImprimirMatriz(matriz);