using MetodosInterpolacion;
using MetodosInterpolacion.Metodos;
using NCalc;

/* 
List<Punto> puntosSeleccionados = new();
puntosSeleccionados.Add(new Punto { X = 1, FX = 0.7651977 });
puntosSeleccionados.Add(new Punto { X = 1.3, FX = 0.6200860 });
puntosSeleccionados.Add(new Punto { X = 1.6, FX = 0.4554022 });
puntosSeleccionados.Add(new Punto { X = 1.9, FX = 0.2818186 });
puntosSeleccionados.Add(new Punto { X = 2.2, FX = 0.1103623 });



DiferenciasDivididas diferenciasDivididas = new(puntosSeleccionados, 4, 1.5);

diferenciasDivididas.CalcularInterpolacion();
ImprimirMatriz(diferenciasDivididas.tablaDiferenciasDivididas);

static void ImprimirMatriz(double[,] matriz)
{
    int filas = matriz.GetLength(0); // Obtiene el número de filas
    int columnas = matriz.GetLength(1); // Obtiene el número de columnas


    for (int j = 0; j < columnas; j++)
    {
        for (int i = 0; i < filas; i++)
        {
            Console.WriteLine(matriz[i, j] + "\t"); // Imprime el valor con tabulación
        }
        Console.WriteLine(); // Salto de línea al final de cada fila
    }
    Console.WriteLine("hola"); // Salto de línea al final de la matriz

}

/* */

Expression variable = new Expression("(x*x)*(2)");
variable.Parameters["x"] = 2;

Console.WriteLine(variable.Evaluate());