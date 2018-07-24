using Newtonsoft.Json;

namespace Commons.Mathematics
{
    public class Matrix3X3 : Matrix
    {
        [JsonConstructor]
        private Matrix3X3(int rows, int columns, double[] flattenedData) : base(rows, columns, flattenedData) {}
        public Matrix3X3() : base(3, 3) { }
    }
}