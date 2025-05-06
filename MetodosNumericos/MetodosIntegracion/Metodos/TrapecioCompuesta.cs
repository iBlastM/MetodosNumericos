using MetodosInterpolacion;
using MetodosInterpolacion.Metodos;
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

        public List<Punto> obtenerPuntosGraficar(double a, double b, int n)
        {
            List<Punto> puntosTrapecio = new();
            double h = (b - a) / n;

            // Subintervalos: puntos superiores de la función
            for (int i = 0; i <= n; i++)
            {
                double x = a + i * h;
                puntosTrapecio.Add(new Punto
                {
                    X = x,
                    FX = Evaluar(expresion, x)
                });

                puntosTrapecio.Add(new Punto
                {
                    X = x,
                    FX = 0
                });

                puntosTrapecio.Add(new Punto
                {
                    X = x,
                    FX = Evaluar(expresion, x)
                });
            }

            // Cierra la figura para relleno: bajamos al eje x desde b a a
            /*for (int i = n; i >= 0; i--)
            {
                double x = a + i * h;
                puntosTrapecio.Add(new Punto
                {
                    X = x,
                    FX = 0
                });
            }*/

            return puntosTrapecio;
        }


        public List<Punto> obtenerPuntosGraficarFuncionReal(double a, double b)
        {
            List<Punto> puntos = new List<Punto>();

            double xIntermedio = a + (b - a) / 2;
            double distanciaEntrePuntos = (b - a) / 2;

            distanciaEntrePuntos = distanciaEntrePuntos / 500;

            // Generar 500 puntos a la izquierda
            for (int i = 500; i > 0; i--)
            {
                double x = xIntermedio - (i * distanciaEntrePuntos);
                puntos.Add(new Punto
                {
                    X = x,
                    FX = Evaluar(expresion, x) // Se calcula FX 
                });
            }

            // Generar el punto base (X inicial)
            puntos.Add(new Punto
            {
                X = xIntermedio,
                FX = Evaluar(expresion, xIntermedio)
            });

            // Generar 500 puntos a la derecha
            for (int i = 1; i <= 500; i++)
            {
                double x = xIntermedio + (i * distanciaEntrePuntos);
                puntos.Add(new Punto
                {
                    X = x,
                    FX = Evaluar(expresion, x) // Se calcula FX con tu método
                });
            }

            return puntos;
        }


    }
}
