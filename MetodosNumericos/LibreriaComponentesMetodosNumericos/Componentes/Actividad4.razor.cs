using SistemasDeEcuaciones;

namespace LibreriaComponentesMetodosNumericos.Componentes;

public partial class Actividad4
{
    private string TempSistemaEcuaciones { get; set; } = string.Empty;
    private string SistemaEcuaciones { get; set; } = string.Empty;
    private double[,]? MatrizAmpliada { get; set; }

    private string mensajeErrorEntradaSistema = string.Empty;

    private void GenerarMatrizAmpliada()
    {
        SistemaEcuaciones = TempSistemaEcuaciones;
        MatrizAmpliada = Helpers.ObtenerMatrizAmpliada(SistemaEcuaciones);
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
        return Helpers.MostrarSistemaEcuaciones(SistemaEcuaciones);
    }

    private string MostrarMatrizAmpliada()
    {
        return Helpers.MostrarMatrizAmpliada(MatrizAmpliada);
    }
}
