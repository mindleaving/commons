using System.Runtime.Serialization;

namespace Commons.Mathematics
{
    [DataContract]
    public class Matrix3X3 : Matrix
    {
        public Matrix3X3() : base(3, 3) { }
    }
}