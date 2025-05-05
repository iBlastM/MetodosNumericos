using MetodosIntegracion.Metodos;
using NCalc;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LibreriaComponentesMetodosNumericos.Componentes;
public partial class Actividad5
{
    string ExpresionInicial = "";
    string a = "0", b = "1";
    int n = 4;
    string MetodoSeleccionado = "trapecio_simple";
    double? resultado;
    string mensajeErrorSubIntervalosNoValidos = string.Empty;
    string mensajeErrorAlEvaluar = string.Empty;

    string Transformar(string expr)
    {
        // Insertar * entre número y letra (2x -> 2*x)
        expr = Regex.Replace(expr, @"(\d)([a-zA-Z])", "$1*$2");

        // Transformar potencias simples a Pow()
        expr = Regex.Replace(expr, @"([a-zA-Z0-9\.]+)\^([a-zA-Z0-9\.\*\+\-\/]+)", "Pow($1,$2)");

        return expr;
    }

    double DetectarPi(string intervalo)
    {
        // Inserta un * entre numero y pi, como en 2pi → 2*pi
        intervalo = Regex.Replace(intervalo, @"(\d)(pi)", "$1*pi", RegexOptions.IgnoreCase);

        // Reemplaza pi por su valor numérico entre parentesis
        intervalo = Regex.Replace(intervalo, @"pi", $"({Math.PI.ToString(CultureInfo.InvariantCulture)})", RegexOptions.IgnoreCase);

        Expression exprHelper = new(intervalo);
        
        return Convert.ToDouble(exprHelper.Evaluate());
    }

    void Calcular()
    {
        string expresion = Transformar(ExpresionInicial);
        double aNumerico = DetectarPi(a);
        double bNumerico = DetectarPi(b);

        try
        {
            mensajeErrorAlEvaluar = string.Empty;
            switch (MetodoSeleccionado)
            {
                case "trapecio_simple":
                    break;

                case "trapecio_compuesto":
                    break;

                case "simpson_13_simple":
                    break;

                case "simpson_13_compuesto":

                    break;

                case "simpson_38_simple":
                    SimpsonTresOctavosSimple simpsonTresOctavosSimple = new SimpsonTresOctavosSimple(expresion);
                    resultado = simpsonTresOctavosSimple.Integrar(aNumerico, bNumerico);
                    break;

                case "simpson_38_compuesto":
                    if (n % 3 != 0) { mensajeErrorSubIntervalosNoValidos = "n debe ser múltiplo de 3 para Simpson 3/8 compuesto"; resultado = null; return; }
                    mensajeErrorSubIntervalosNoValidos = string.Empty;
                    SimpsonTresOctavosCompuesta simpsonTresOctavosCompuesta = new SimpsonTresOctavosCompuesta(expresion);
                    resultado = simpsonTresOctavosCompuesta.Integrar(aNumerico, bNumerico, n);
                    break;
            }
        }
        catch (Exception ex)
        {
            resultado = null;
            mensajeErrorAlEvaluar = $"Error al evaluar: {ex.Message}";
        }
    }
}