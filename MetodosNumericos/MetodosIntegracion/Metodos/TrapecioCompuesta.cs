using NCalc;

namespace MetodosIntegracion.Metodos
{
    public class TrapecioCompuesta(string expr) : MetodoIntegracion
    {
        Expression expresion = new Expression(expr);

        public double Integrar(double a, double b, int n)
        {
            double h = (b - a) / n;
            double sum = 0;

            // Sumar f(x1) + f(x2) + ... + f(x_{n-1})
            for (int i = 1; i < n; i++)
            {
                double xi = a + i * h;
                sum += Evaluar(expresion, xi);
            }

            // Aplicar la fórmula del trapecio compuesto
            double resultado = (h / 2) * (Evaluar(expresion, a) + 2 * sum + Evaluar(expresion, b));

            return resultado;
        }
    }
}
