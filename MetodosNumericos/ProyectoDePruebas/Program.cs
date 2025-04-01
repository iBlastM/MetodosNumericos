using SistemasDeEcuaciones;
using SistemasDeEcuaciones.Metodos;

Helpers helpers = new();


Console.WriteLine("0.5x + y = 2, y - x = -1");
double[,] matriz = helpers.ObtenerMatrizAmpliada("0.5x + y = 2, y - x = -1");

ImprimirMatriz(matriz);

SustitucionAtras sustitucionAtras = new(helpers.terminos);

if (sustitucionAtras.CalcularSoluciones(matriz))
{
    Console.WriteLine("Soluciones encontradas:");
    foreach (Termino termino in helpers.terminos)
    {
        Console.WriteLine($"{termino.Variable} = {termino.Solucion}");
    }
    foreach (Paso paso in sustitucionAtras.pasos)
    {
        Console.WriteLine(paso.operacion);
        ImprimirMatriz(paso.matriz);
    }
}
else
{
    Console.WriteLine("No se encontraron soluciones");
}



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
