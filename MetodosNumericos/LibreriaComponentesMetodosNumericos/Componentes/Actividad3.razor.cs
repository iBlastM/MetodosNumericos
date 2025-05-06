namespace LibreriaComponentesMetodosNumericos.Componentes;
using MetodosInterpolacion;
using MetodosInterpolacion.Metodos;
using Plotly.Blazor;
using Plotly.Blazor.Traces;
using Plotly.Blazor.Traces.ScatterCarpetLib;
public partial class Actividad3
{
    private List<Punto> puntos = new();
    private Punto nuevoPunto = new();
    private int gradoInterpolacion = 1; // Grado del polinomio de interpolación
    private List<Punto> puntosSeleccionados => puntos.Where(p => p.Seleccionado).ToList(); //Lista de puntos con los que se va a trabajar
    private double xInterpolar; // Valor de X para el cual se quiere interpolar
    private double? resultadoInterpolacion;
    private string metodoSeleccionado = "Lagrange"; // Método por defecto
    private string mensajeErrorPuntoExistente = string.Empty;
    private string mensajeErrorGradoPolinomioErroneo = string.Empty;
    private string mensajeErrorDatosVacios = string.Empty;
    private double[,] matrizTablaDiferenciasDivididas;
    private double[,] matrizTablaNeville;
    private string polinomioInterpolacion;
    private bool seleccionarTodos;

    private bool SeleccionarTodos
    {
        get => seleccionarTodos;
        set
        {
            seleccionarTodos = value;
            foreach (var punto in puntos)
            {
                punto.Seleccionado = seleccionarTodos;
            }
        }
    }

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
                    Name = "Interpolación",
                    Mode = Plotly.Blazor.Traces.ScatterLib.ModeFlag.Lines | Plotly.Blazor.Traces.ScatterLib.ModeFlag.Markers,
                    X = new List<object>{},
                    Y = new List<object>{}
                }
            };
    }
    private void AgregarPunto()
    {
        if (puntos.Any(p => p.X == nuevoPunto.X))
        {
            mensajeErrorPuntoExistente = $"El punto con X = {nuevoPunto.X} ya está registrado.";
            return;
        }

        mensajeErrorPuntoExistente = string.Empty;
        puntos.Add(new Punto { X = nuevoPunto.X, FX = nuevoPunto.FX });
        puntos = puntos.OrderBy(p => p.X).ToList();
        nuevoPunto = new Punto();

    }

    private void EliminarPunto(Punto punto)
    {
        puntos.Remove(punto);
    }

    private void ActualizarGrafica()
    {
        chart.React();
    }
    private void CalcularInterpolacion()
    {
        if (gradoInterpolacion > puntosSeleccionados.Count - 1 || gradoInterpolacion < 0)
        {
            mensajeErrorGradoPolinomioErroneo = $"El grado del polinomio debe ser menor a la cantidad de puntos seleccionados y mayor o igual que 0.";
            resultadoInterpolacion = null;
            return;
        }
        mensajeErrorGradoPolinomioErroneo = string.Empty;
        // Aquí se llama al método correspondiente según la selección

        if (puntos.Count == 0 || puntosSeleccionados.Count == 0)
        {
            mensajeErrorDatosVacios = $"La tabla de datos esta vacia o no hay ningun punto seleccionado.";
            return;
        }
        mensajeErrorDatosVacios = string.Empty;

        switch (metodoSeleccionado)
        {
            case "Lagrange":
                resultadoInterpolacion = InterpolacionLagrange();
                break;
            case "DiferenciasDivididas":
                resultadoInterpolacion = DiferenciasDivididas();
                ActualizarGrafica();
                break;
            case "Neville":
                resultadoInterpolacion = MetodoNeville();
                ActualizarGrafica();
                break;
        }

    }

    private double InterpolacionLagrange()
    {
        mostrarGrafica = true;
        InterpolacionLagrange interpolacionLagrange = new(puntosSeleccionados, gradoInterpolacion, xInterpolar);

        double resultado = interpolacionLagrange.CalcularInterpolacion();
        polinomioInterpolacion = interpolacionLagrange.polinomioInterpolacion;


        var x = interpolacionLagrange.puntosGraficar.Select(p => (object)p.X).ToList();
        var y = interpolacionLagrange.puntosGraficar.Select(p => (object)p.FX).ToList();
        data = new List<ITrace>
    {
        new Scatter
        {
            Name = "Polinomio",
            Mode = Plotly.Blazor.Traces.ScatterLib.ModeFlag.Lines,
            X = x,
            Y = y,
            Text = "Polinomio", // Etiquetas,
            TextPosition = (Plotly.Blazor.Traces.ScatterLib.TextPositionEnum?)TextPositionEnum.MiddleLeft
        },
        new Scatter
        {
            Name = "Punto interpolado",
            X = new List<object> { xInterpolar }, // Datos del eje X
            Y = new List<object> { resultado }, // Datos del eje Y
            Mode = Plotly.Blazor.Traces.ScatterLib.ModeFlag.Markers, // Modo de la gráfica (líneas y marcadores)
            Text = "Punto interpolado", // Etiquetas
            TextPosition = (Plotly.Blazor.Traces.ScatterLib.TextPositionEnum?)TextPositionEnum.MiddleLeft
        }

    };

        chart.Update(data, layout);

        return resultado; // Retorna el valor calculado
    }

    private double DiferenciasDivididas()
    {
        mostrarGrafica = true;
        DiferenciasDivididas diferenciasDivididas = new(puntosSeleccionados, gradoInterpolacion, xInterpolar);
        matrizTablaDiferenciasDivididas = diferenciasDivididas.tablaDiferenciasDivididas;

        double resultado = diferenciasDivididas.CalcularInterpolacion();
        polinomioInterpolacion = diferenciasDivididas.polinomioInterpolacion;

        var x = diferenciasDivididas.puntosGraficar.Select(p => (object)p.X).ToList();
        var y = diferenciasDivididas.puntosGraficar.Select(p => (object)p.FX).ToList();
        data = new List<ITrace>
        {
        new Scatter
        {
            Name = "Polinomio",
            Mode = Plotly.Blazor.Traces.ScatterLib.ModeFlag.Lines,
            X = x,
            Y = y,
            Text = "Polinomio", // Etiquetas,
            TextPosition = (Plotly.Blazor.Traces.ScatterLib.TextPositionEnum?)TextPositionEnum.MiddleLeft
        },
        new Scatter
        {
            Name = "Punto interpolado",
            X = new List<object> { xInterpolar }, // Datos del eje X
            Y = new List<object> { resultado }, // Datos del eje Y
            Mode = Plotly.Blazor.Traces.ScatterLib.ModeFlag.Markers, // Modo de la gráfica (líneas)
            Text = "Punto interpolado", // Etiquetas
            TextPosition = (Plotly.Blazor.Traces.ScatterLib.TextPositionEnum?)TextPositionEnum.MiddleLeft
        }

    };

        chart.Update(data, layout);
        return resultado; // Retorna el valor calculado
    }

    private double MetodoNeville()
    {
        mostrarGrafica = true;
        MetodoNeville neville = new(puntosSeleccionados, gradoInterpolacion, xInterpolar);
        matrizTablaNeville = neville.tablaNeville;
        double resultado = neville.CalcularInterpolacion();
        polinomioInterpolacion = neville.polinomioInterpolacion;

        var x = neville.puntosGraficar.Select(p => (object)p.X).ToList();
        var y = neville.puntosGraficar.Select(p => (object)p.FX).ToList();
        data = new List<ITrace>
    {
        new Scatter
        {
            Name = "Polinomio",
            Mode = Plotly.Blazor.Traces.ScatterLib.ModeFlag.Lines,
            X = x,
            Y = y
        },
        new Scatter
        {
            Name = "Punto interpolado",
            X = new List<object> { xInterpolar },
            Y = new List<object> { resultado },
            Mode = Plotly.Blazor.Traces.ScatterLib.ModeFlag.Markers
        }
    };

        chart.Update(data, layout);
        return resultado;
    }
}