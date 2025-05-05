using SistemasDeEcuaciones;
using SistemasDeEcuaciones.Metodos;

Helpers helpers = new();


NCalc.Expression exp = new NCalc.Expression("2*Pow(2, 2)");
exp.Parameters["x"] = 2;
exp.Parameters["e"] = 2.718281828459045;
var resultado = exp.Evaluate();
Console.WriteLine(resultado);



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
