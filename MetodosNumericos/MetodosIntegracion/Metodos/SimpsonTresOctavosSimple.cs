using NCalc;

namespace MetodosIntegracion.Metodos;
public class SimpsonTresOctavosSimple(string expr) : MetodoIntegracion
{
    
    Expression expresion = new Expression(expr);
    public double Integrar(double a, double b)
    {
        double h38 = (b - a) / 3;
        double resultado = 3 * h38 / 8 * (Evaluar(expresion, a) + 3 * Evaluar(expresion, a + h38) + 3 * 
            Evaluar(expresion, a + 2 * h38) + Evaluar(expresion, b));
        return resultado;
    }
}
