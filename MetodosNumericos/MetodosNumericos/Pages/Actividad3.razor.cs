using MetodosInterpolacion;
using MetodosInterpolacion.Metodos;

namespace MetodosNumericos.Pages;
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
    private void CalcularInterpolacion()
    {
        if(gradoInterpolacion > puntosSeleccionados.Count - 1 || gradoInterpolacion < 0)
        {
            mensajeErrorGradoPolinomioErroneo = $"El grado del polinomio debe ser menor a la cantidad de puntos seleccionados y mayor o igual que 0.";
            resultadoInterpolacion = null;
            return;
        }
        mensajeErrorGradoPolinomioErroneo = string.Empty;
        // Aquí se llama al método correspondiente según la selección
        switch (metodoSeleccionado)
        {
            case "Lagrange":
                resultadoInterpolacion = InterpolacionLagrange();
                break;
            case "DiferenciasDivididas":
                resultadoInterpolacion = DiferenciasDivididas();
                break;
            case "Neville":
                resultadoInterpolacion = MetodoNeville();
                break;
        }
    }

    private double InterpolacionLagrange()
    {
        InterpolacionLagrange interpolacionLagrange = new(puntosSeleccionados, gradoInterpolacion, xInterpolar);
        interpolacionLagrange.Calcular();
        // Implementa aquí el método de interpolación de Lagrange
        return 0; // Retorna el valor calculado
    }

    private double DiferenciasDivididas()
    {
        Console.WriteLine(puntosSeleccionados.Count);
        DiferenciasDivididas diferenciasDivididas = new(puntosSeleccionados, gradoInterpolacion, xInterpolar);
        return 0; // Retorna el valor calculado
    }

    private double MetodoNeville()
    {
        // Implementa aquí el método de Neville
        return 0; // Retorna el valor calculado
    }
}