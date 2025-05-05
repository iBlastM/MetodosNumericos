using NCalc;

namespace MetodosIntegracion.Metodos;
public class SimpsonTresOctavosCompuesta(string expr) : MetodoIntegracion
{
    Expression expresion = new Expression(expr);
    public double Integrar(double a, double b, int n)
    {
        double h383 = (b - a) / n;
        double sum383 = 0;
        for (int i = 1; i < n; i++)
        {
            int coef = (i % 3 == 0) ? 2 : 3;
            sum383 += coef * Evaluar(expresion, a + i * h383);
        }
        double resultado = 3 * h383 / 8 * (Evaluar(expresion, a) + sum383 + Evaluar(expresion, b));

        return resultado;
    }
}
