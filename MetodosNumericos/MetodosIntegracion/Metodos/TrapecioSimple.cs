using MetodosInterpolacion;
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

        public List<Punto> obtenerPuntosGraficar(double a, double b)
        {
            return new List<Punto>
            {
                new Punto { X = a, FX = 0 },
                new Punto { X = a, FX = Evaluar(expresion, a) },
                new Punto { X = b, FX = Evaluar(expresion, b) },
                new Punto { X = b, FX = 0 },
                new Punto { X = a, FX = 0 } // Cierra el trapecio
            };
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
