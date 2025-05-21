namespace SistemasDeEcuaciones.Metodos
{
    public class PasoLU
    {
        public string operacion { get; set; }

        public double[,] matrizL { get; set; }

        public double[,] matrizU { get; set; }

        public double[] vectorY { get; set; }

        public double[] vectorX { get; set; }
        public double[,] matrizOriginal { get; set; }
    }
}
