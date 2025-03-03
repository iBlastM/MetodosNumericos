using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodosInterpolacion.Metodos;
public class InterpolacionLagrange(List<Punto> Puntos, int gradoInterpolacion, double xInterpolado)
{
    List<Punto> puntos = Puntos;
    public double[,] tablaDiferenciasDivididas { get; private set; } = new double[Puntos.Count + Puntos.Count - 1, 1 + Puntos.Count];
    public double[] coeficientes { get; private set; } = new double[Puntos.Count];
    public string polinomioInterpolacion { get; private set; }
    public List<Punto> puntosGraficar { get; private set; }
    public double CalcularInterpolacion()
    {
        polinomioInterpolacion = $"{gradoInterpolacion} P(x) = ";
        GenerarCadenaPolinomio();
        double resultadoInterpolacion = CalcularPuntoInterpolado(xInterpolado);
        return resultadoInterpolacion;
    }

    private double CalcularPuntoInterpolado(double x)
    {
        double result = 0;
        double aux = 1;
        for (int k = 0; k < gradoInterpolacion + 1; k++)
        {
            for (int i = 0; i < gradoInterpolacion + 1; i++)
            {
                if (i != k)
                {
                    aux *= (x -  puntos[i].X) /( puntos[k].X -  puntos[i].X) ;
                }

            }
            aux *= puntos[k].FX;
            result += aux;
            aux = 1;
        }
        return result;
    }

    private void GenerarCadenaPolinomio()
    {
        for (int k = 0; k < gradoInterpolacion + 1; k++)
        {
            for (int i =0; i<gradoInterpolacion+1; i++)
            {
                if (i != k)
                {
                    polinomioInterpolacion += $"( x - {puntos[i].X} )";
                }

            }
            polinomioInterpolacion += "/";
            for (int i = 0; i < gradoInterpolacion + 1; i++)
            {
                if (i != k)
                {
                    polinomioInterpolacion += $"( {puntos[k].X} - {puntos[i].X} )";
                }

            }
            if (k == gradoInterpolacion)
            {
                polinomioInterpolacion += $"{puntos[k].FX}";
            }
            else
            {
                polinomioInterpolacion += $"{puntos[k].FX} + ";
            }
           
           
        }

    }

}
