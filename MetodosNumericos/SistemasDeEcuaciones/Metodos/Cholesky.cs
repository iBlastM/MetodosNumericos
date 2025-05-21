namespace SistemasDeEcuaciones.Metodos;
public class Cholesky(List<Termino> terminos) : IMetodo
{
    public List<Termino> Terminos = terminos;
    public List<Paso> pasos = new();

    public bool CalcularSoluciones(double[,] matriz)
    {
        int n = matriz.GetLength(0);
        double[,] L = new double[n, n];

        // Factorización de Cholesky: A = L * L^T
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                double suma = 0;
                for (int k = 0; k < j; k++)
                {
                    suma += L[i, k] * L[j, k];
                }

                if (i == j)
                {
                    double valor = matriz[i, i] - suma;
                    if (valor <= 0) return false;
                    L[i, j] = Math.Sqrt(valor);
                    pasos.Add(new Paso
                    {
                        operacion = $"L[{i + 1},{j + 1}] = sqrt({matriz[i, i]} - {suma}) = {L[i, j]}",
                        matriz = (double[,])L.Clone()
                    });
                }
                else
                {
                    L[i, j] = (1.0 / L[j, j]) * (matriz[i, j] - suma);
                    pasos.Add(new Paso
                    {
                        operacion = $"L[{i + 1},{j + 1}] = ({matriz[i, j]} - {suma}) / {L[j, j]} = {L[i, j]}",
                        matriz = (double[,])L.Clone()
                    });
                }
            }
        }

        // Sustitución hacia adelante: L * y = b
        double[] y = new double[n];
        for (int i = 0; i < n; i++)
        {
            double suma = 0;
            for (int j = 0; j < i; j++)
            {
                suma += L[i, j] * y[j];
            }
            y[i] = (matriz[i, n] - suma) / L[i, i];
            pasos.Add(new Paso
            {
                operacion = $"y[{i + 1}] = ({matriz[i, n]} - {suma}) / {L[i, i]} = {y[i]}",
                matriz = (double[,])L.Clone()
            });
        }

        // Sustitución hacia atrás: L^T * x = y
        for (int i = n - 1; i >= 0; i--)
        {
            double suma = 0;
            for (int j = i + 1; j < n; j++)
            {
                suma += L[j, i] * Terminos[j].Solucion;
            }
            Terminos[i].Solucion = (y[i] - suma) / L[i, i];
            pasos.Add(new Paso
            {
                operacion = $"x[{Terminos[i].Variable}] = ({y[i]} - {suma}) / {L[i, i]} = {Terminos[i].Solucion}",
                matriz = (double[,])L.Clone()
            });
        }

        return true;
    }
}
