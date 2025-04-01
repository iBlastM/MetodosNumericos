using MetodosInterpolacion;
using SistemasDeEcuaciones;
using SistemasDeEcuaciones.Metodos;

namespace LibreriaComponentesMetodosNumericos.Componentes;

public partial class Actividad4
{
    private string TempSistemaEcuaciones { get; set; } = string.Empty;
    private string SistemaEcuaciones { get; set; } = string.Empty;
    private double[,]? MatrizAmpliada { get; set; }

    private double[,]? MatrizAmpliadaInicial { get; set; }

    private string mensajeErrorEntradaSistema = string.Empty;
    private string mensajeErrorSistemaSinSolucion = string.Empty;
    private bool errorMatrizNoCuadrada = false;
    private Helpers helpers = new Helpers();
    private string metodoSeleccionado = "Eliminación Gaussiana con sustitución hacia atrás";
    private List<Paso> pasos = new();
    private bool tieneSolucion = false;

    private void GenerarMatrizAmpliada()
    {
        SistemaEcuaciones = TempSistemaEcuaciones;
        MatrizAmpliada = helpers.ObtenerMatrizAmpliada(SistemaEcuaciones);
        MatrizAmpliadaInicial = MatrizAmpliada.Clone() as double[,];
        if(MatrizAmpliada == null)
        {
            mensajeErrorEntradaSistema = "Entrada no valida, verifica que el sistema ingresado cumpla con el formato requerido.";
        }
        else
        {
            mensajeErrorEntradaSistema = string.Empty;
        }

    }

    private string MostrarSistemaEcuaciones()
    {
        return helpers.MostrarSistemaEcuaciones(SistemaEcuaciones);
    }

    private string MostrarMatrizAmpliada(double[,] matrizAmpliada)
    {
        return helpers.MostrarMatrizAmpliada(matrizAmpliada);
    }

    private void CalcularSoluciones()
    {

        if (MatrizAmpliada.GetLength(0) != helpers.terminos.Count)
        {
            errorMatrizNoCuadrada = true;
            mensajeErrorSistemaSinSolucion = string.Empty;
            tieneSolucion = false;
            return;
        }

        switch (metodoSeleccionado)
        {
            case "HaciaAtras":
                SustitucionAtras sustitucionAtras = new SustitucionAtras(helpers.terminos);
                if(tieneSolucion = sustitucionAtras.CalcularSoluciones(MatrizAmpliada))
                {

                    pasos = sustitucionAtras.pasos;
                    Console.WriteLine(pasos.Count);
                }
                
                break;
            case "PivoteoMaximo":

                break;
            case "PivoteoEscalado":

                break;
        }

        if(!tieneSolucion)
        {
            mensajeErrorSistemaSinSolucion = "El sistema no tiene solución o tiene soluciones infinitas";
        }
        else
        {
            mensajeErrorSistemaSinSolucion = string.Empty;
        }

    }
}
