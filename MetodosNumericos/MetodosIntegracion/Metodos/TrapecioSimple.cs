using NCalc;

namespace MetodosIntegracion.Metodos
{
    public class TrapecioSimple(string expr) : MetodoIntegracion
    {
        Expression expresion = new Expression(expr);

        public double Integrar(double a, double b)
        {
            double fa = Evaluar(expresion, a);
            double fb = Evaluar(expresion, b);
            double resultado = (b - a) / 2 * (fa + fb);
            return resultado;
        }
    }
}
