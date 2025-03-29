using MetodosInterpolacion;
using SistemasDeEcuaciones;
using SistemasDeEcuaciones.Metodos;

namespace LibreriaComponentesMetodosNumericos.Componentes;

public partial class Actividad4
{
    private string TempSistemaEcuaciones { get; set; } = string.Empty;
    private string SistemaEcuaciones { get; set; } = string.Empty;
    private double[,]? MatrizAmpliada { get; set; }

    private string mensajeErrorEntradaSistema = string.Empty;
    private Helpers helpers = new Helpers();
    private string metodoSeleccionado = "Eliminación Gaussiana con sustitución hacia atrás";
    private List<Paso> pasos = new();
    private bool tieneSolucion = false;

    private void GenerarMatrizAmpliada()
    {
        SistemaEcuaciones = TempSistemaEcuaciones;
        MatrizAmpliada = helpers.ObtenerMatrizAmpliada(SistemaEcuaciones);
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

    private string MostrarMatrizAmpliada()
    {
        return helpers.MostrarMatrizAmpliada(MatrizAmpliada);
    }

    private void CalcularSoluciones()
    {

        switch (metodoSeleccionado)
        {
            case "HaciaAtras":
                SustitucionAtras sustitucionAtras = new SustitucionAtras(helpers.terminos);
                if(tieneSolucion = sustitucionAtras.CalcularSoluciones(MatrizAmpliada))
                {
                    pasos = sustitucionAtras.pasos;
                }
                
                break;
            case "PivoteoMaximo":

                break;
            case "PivoteoEscalado":

                break;
        }

    }
}
