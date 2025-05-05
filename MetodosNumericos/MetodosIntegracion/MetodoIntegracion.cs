using NCalc;

namespace MetodosIntegracion;
public abstract class MetodoIntegracion
{
    public double Evaluar(Expression expr, double x)
    {
        expr.Parameters["e"] = Math.E;
        expr.Parameters["x"] = x;
        return Convert.ToDouble(expr.Evaluate());
    }

}
