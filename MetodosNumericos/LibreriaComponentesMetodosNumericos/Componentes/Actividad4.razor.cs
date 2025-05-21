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
    private string mensajeErrorNoHayMatriz = string.Empty;
    private bool errorMatrizNoCuadrada = false;

    private Helpers helpers;
    private string metodoSeleccionado = "Eliminación Gaussiana con sustitución hacia atrás";
    private List<Paso> pasos;
    private bool tieneSolucion = false;

    private void GenerarMatrizAmpliada()
    {
        MatrizAmpliada = new double[0, 0];
        MatrizAmpliadaInicial = new double[0, 0];
        helpers = new Helpers();
        pasos = new List<Paso>();
        tieneSolucion = false;

        SistemaEcuaciones = TempSistemaEcuaciones;
        MatrizAmpliada = helpers.ObtenerMatrizAmpliada(SistemaEcuaciones);
        
        if(MatrizAmpliada == null)
        {
            mensajeErrorEntradaSistema = "Entrada no valida, verifica que el sistema ingresado cumpla con el formato requerido.";
            return;
        }
        else
        {
            mensajeErrorEntradaSistema = string.Empty;
        }
        MatrizAmpliadaInicial = MatrizAmpliada.Clone() as double[,];

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
        if (MatrizAmpliada == null)
        {
            mensajeErrorNoHayMatriz = "No se ha introducido ninguna matriz.";
            return;
        }
        else
        {
            mensajeErrorNoHayMatriz = string.Empty;
        }


        if (MatrizAmpliada.GetLength(0) != helpers.terminos.Count)
        {
            errorMatrizNoCuadrada = true;
            mensajeErrorSistemaSinSolucion = string.Empty;
            tieneSolucion = false;
            return;
        }

        errorMatrizNoCuadrada = false;

        switch (metodoSeleccionado)
        {
            case "HaciaAtras":
                SustitucionAtras sustitucionAtras = new SustitucionAtras(helpers.terminos);
                if(tieneSolucion = sustitucionAtras.CalcularSoluciones(MatrizAmpliada.Clone() as double[,]))
                {
                    pasos = sustitucionAtras.pasos;
                }
                
                break;
            case "PivoteoMaximo":
                PivoteoMaximo pivoteoMaximo = new(helpers.terminos);
                if (tieneSolucion = pivoteoMaximo.CalcularSoluciones(MatrizAmpliada.Clone() as double[,]))
                {
                    pasos = pivoteoMaximo.pasos;
                }
                break;
            case "PivoteoEscalado":
                PivoteoEscalado pivoteoEscalado = new(helpers.terminos);
                if (tieneSolucion = pivoteoEscalado.CalcularSoluciones(MatrizAmpliada.Clone() as double[,]))
                {
                    pasos = pivoteoEscalado.pasos;
                }
                break;
            case "Cholesky":
                Cholesky cholesky = new(helpers.terminos);
                Console.WriteLine("Este es un mensaje en la consola");
                if (tieneSolucion = cholesky.CalcularSoluciones(MatrizAmpliada.Clone() as double[,]))
                {
                    pasos = cholesky.pasos;
                }
                break;


        }

        if (!tieneSolucion)
        {
            mensajeErrorSistemaSinSolucion = "El sistema no tiene solución o tiene soluciones infinitas";
        }
        else
        {
            mensajeErrorSistemaSinSolucion = string.Empty;
        }

    }
}
