using MetodosIntegracion.Metodos;
using MetodosInterpolacion;
using NCalc;
using Plotly.Blazor;
using Plotly.Blazor.LayoutLib.MapBoxLib.LayerLib;
using Plotly.Blazor.Traces;
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

    private bool mostrarGrafica = false;
    PlotlyChart chart = new();
    Plotly.Blazor.Layout layout;
    Config config;
    IList<ITrace> data;

    protected override void OnInitialized()
    {
        config = new Config();
        layout = new();
        data = new List<ITrace>
            {
                new Scatter
                {
                    Name = "Polinomio",
                    Mode = Plotly.Blazor.Traces.ScatterLib.ModeFlag.Lines,
                    X = new List<object>{},
                    Y = new List<object>{},
                    Fill = Plotly.Blazor.Traces.ScatterLib.FillEnum.ToZeroY,
                },
                new Scatter
                {
                    Name = "Limites",
                    Mode = Plotly.Blazor.Traces.ScatterLib.ModeFlag.Markers,
                    X = new List<object>{},
                    Y = new List<object>{}
                },
                new Scatter
                {
                    Name = "Funcion Real",
                    Mode = Plotly.Blazor.Traces.ScatterLib.ModeFlag.Lines,
                    X = new List<object>{},
                    Y = new List<object>{},
                    Fill = Plotly.Blazor.Traces.ScatterLib.FillEnum.ToZeroY
                }
            };
    }

    string Transformar(string expr)
    {
        // Inserta * entre número y letra (2x -> 2*x)
        expr = Regex.Replace(expr, @"(\d)([a-zA-Z])", "$1*$2");

        // Potencias con paréntesis en el exponente: e^(x^2) -> Pow(e, Pow(x,2))
        expr = Regex.Replace(expr, @"([a-zA-Z0-9\.]+)\^\(([^)]+)\)", "Pow($1,$2)");

        // Potencias simples: x^2 -> Pow(x,2)
        expr = Regex.Replace(expr, @"([a-zA-Z0-9\.]+)\^([a-zA-Z0-9\.]+)", "Pow($1,$2)");

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

    public async Task Calcular()
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
                    TrapecioSimple trapecioSimple = new TrapecioSimple(expresion);
                    resultado = trapecioSimple.Integrar(aNumerico, bNumerico);
                    break;

                case "trapecio_compuesto":
                    TrapecioCompuesta trapecioCompuesta = new TrapecioCompuesta(expresion);
                    resultado = trapecioCompuesta.Integrar(aNumerico, bNumerico, n);
                    break;

                case "simpson_13_simple":
                    break;

                case "simpson_13_compuesto":

                    break;

                case "simpson_38_simple":
                    SimpsonTresOctavosSimple simpsonTresOctavosSimple = new SimpsonTresOctavosSimple(expresion);
                    resultado = simpsonTresOctavosSimple.Integrar(aNumerico, bNumerico);
                    List<Punto> puntosSimpson = simpsonTresOctavosSimple.obtenerPuntosGraficar(aNumerico, bNumerico);
                    List<Punto> puntosFuncionReal = simpsonTresOctavosSimple.obtenerPuntosGraficarFuncionReal(aNumerico, bNumerico);
                    LimpiarGrafica();
                    var scatter = data[0] as Scatter;
                    foreach (var punto in puntosSimpson)
                    {
                        scatter?.X.Add(punto.X);
                        scatter?.Y.Add(punto.FX);
                    }

                    var scatterFuncionReal = data[2] as Scatter;
                    foreach (var punto in puntosFuncionReal)
                    {
                        scatterFuncionReal?.X.Add(punto.X);
                        scatterFuncionReal?.Y.Add(punto.FX);
                    }
                    await chart.React();
                    await InvokeAsync(StateHasChanged);

                    var scatter2 = data[1] as Scatter;
                    scatter2?.X.Add(puntosSimpson[0].X);
                    scatter2?.Y.Add(puntosSimpson[0].FX);
                    scatter2?.X.Add(puntosSimpson[puntosSimpson.Count - 1].X);
                    scatter2?.Y.Add(puntosSimpson[puntosSimpson.Count - 1].FX);

                    await chart.React();
                    await InvokeAsync(StateHasChanged);


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

    private void LimpiarGrafica()
    {
        if (data[0] is Scatter scatter)
        {
            scatter.X.Clear();
            scatter.Y.Clear();
        }

        if (data[1] is Scatter scatter2)
        {
            scatter2.X.Clear();
            scatter2.Y.Clear();
        }

        if (data[2] is Scatter scatter3)
        {
            scatter3.X.Clear();
            scatter3.Y.Clear();
        }
    }
}