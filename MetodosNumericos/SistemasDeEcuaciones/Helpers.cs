using System.Text.RegularExpressions;

namespace SistemasDeEcuaciones;
public static class Helpers
{
    public static bool EsSistemaValido(string sistema)
    {
        if (string.IsNullOrWhiteSpace(sistema))
            return false;

        // Expresión regular para validar ecuaciones del tipo "ax + by + ... + nz = c"
        string patronEcuacion = @"^\s*([\+\-]?\s*\d*[a-zA-Z](\s*[\+\-]\s*\d*[a-zA-Z])*)\s*=\s*[\+\-]?\d+\s*$";

        // Dividir por comas para analizar cada ecuación individualmente
        string[] ecuaciones = sistema.Split(',');

        foreach (string ecuacion in ecuaciones)
        {
            if (!Regex.IsMatch(ecuacion, patronEcuacion))
                return false;
        }

        return true;
    }

    public static double[,] ObtenerMatrizAmpliada(string sistemaDeEcuaciones)
    {
        List<Termino> terminos = new List<Termino>();

        if (!EsSistemaValido(sistemaDeEcuaciones))
            return null;

        double[,] matriz;

        string[] ecuaciones = sistemaDeEcuaciones.Split(',');

        double[] resultados = new double[ecuaciones.Length];

        int nFila = 0;

        foreach (string ecuacion in ecuaciones)
        {
            string ecuacionTrimmed = ecuacion.Replace(" ", "");
            string[] partes = ecuacionTrimmed.Split('=');
            char[] caracteres = partes[0].ToCharArray();
            int i = 0;
            string numero = "";
            string variable = "";
            while (i < caracteres.Length)
            {
                if(caracteres[i] == '+' || caracteres[i] == '-')
                {
                    numero += caracteres[i];
                    i++;
                }
                else if (char.IsDigit(caracteres[i]))
                {
                    numero += caracteres[i];
                    i++;
                }
                else
                {
                    if(numero == "-" || numero == "+" || numero == "")
                    {
                        numero += "1";
                    }
                    variable = caracteres[i].ToString();

                    AgregarTermino(terminos, new Termino(ecuaciones.Length, variable), numero, nFila);
                    numero = "";
                    variable = "";
                    i++;
                }
            }

            resultados[nFila] = double.Parse(partes[1]);

            nFila++;
        }

        terminos = terminos.OrderBy(t => t.Variable).ToList();

        matriz = new double[ecuaciones.Length, terminos.Count + 1];

        for (int i = 0; i < ecuaciones.Length; i++)
        {
            for (int j = 0; j < terminos.Count; j++)
            {
                matriz[i, j] = terminos[j].Coeficientes[i];
            }
            matriz[i, terminos.Count] =  resultados[i];
        }

        return matriz;
    }

    static void AgregarTermino(List<Termino> lista, Termino nuevoTermino, string numero, int nFila)
    {
        if (!lista.Any(t => t.Variable == nuevoTermino.Variable))
        {
            lista.Add(nuevoTermino);
            var termino = lista.FirstOrDefault(t => t.Variable == nuevoTermino.Variable);
            if (termino != null)
            {
                termino.Coeficientes[nFila] += double.Parse(numero);
            }
        }
        else
        {
            var termino = lista.FirstOrDefault(t => t.Variable == nuevoTermino.Variable);
            if (termino != null)
            {
                termino.Coeficientes[nFila] += double.Parse(numero);
            }
        }
    }

    public static string MostrarSistemaEcuaciones(string SistemaEcuaciones)
    {
        if (string.IsNullOrEmpty(SistemaEcuaciones))
            return string.Empty;

        var ecuaciones = SistemaEcuaciones.Split(',', StringSplitOptions.TrimEntries);
        return string.Join("\n", ecuaciones);
    }

    public static string MostrarMatrizAmpliada(double[,] MatrizAmpliada)
    {
        if (MatrizAmpliada == null) return string.Empty;

        int filas = MatrizAmpliada.GetLength(0);
        int cols = MatrizAmpliada.GetLength(1);

        // Determinar el ancho máximo de cada columna para una alineación uniforme
        int[] maxAnchoColumna = new int[cols];

        for (int j = 0; j < cols; j++)
        {
            maxAnchoColumna[j] = 0;
            for (int i = 0; i < filas; i++)
            {
                maxAnchoColumna[j] = Math.Max(maxAnchoColumna[j], MatrizAmpliada[i, j].ToString().Length);
            }
        }

        var filasTexto = new List<string>();

        for (int i = 0; i < filas; i++)
        {
            var elementos = new List<string>();

            for (int j = 0; j < cols; j++)
            {
                string valor = MatrizAmpliada[i, j].ToString().PadLeft(maxAnchoColumna[j]);

                if (j == cols - 1)
                    elementos.Add("| " + valor);  // Separador para la última columna
                else
                    elementos.Add(valor);
            }

            filasTexto.Add("( " + string.Join("  ", elementos) + " )");
        }

        return string.Join("\n", filasTexto);
    }
}
