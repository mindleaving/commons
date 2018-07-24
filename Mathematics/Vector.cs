using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace Commons.Mathematics
{
    public class Vector : IEquatable<Vector>
    {
        [JsonIgnore]
        public int Dimension => Data.Length;
        [JsonProperty]
        public double[] Data { get; private set; }

        [JsonConstructor]
        public Vector(params double[] data)
        {
            Data = new double[data.Length];
            Set(data);
        }

        public void Set(double[] values)
        {
            if (values.Length != Dimension)
                throw new ArgumentException($"Data provided must have same length as vector, {Dimension}, but was {values.Length}");

            Array.Copy(values, Data, values.Length);
        }

        public static implicit operator double[](Vector v)
        {
            return v.Data;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            if (v1.Dimension != v2.Dimension)
                throw new InvalidOperationException("Cannot add vectors of different length");
            var additionResult = v1.Data.Sum(v2.Data);
            if (v1.Dimension == 2)
                return new Vector2D(additionResult);
            if (v1.Dimension == 3)
                return new Vector3D(additionResult);
            return new Vector(additionResult);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            if (v1.Dimension != v2.Dimension)
                throw new InvalidOperationException("Cannot add vectors of different length");
            var subtractionResult = v1.Data.Sum(v2.Data.ScalarMultiply(-1));
            if (v1.Dimension == 2)
                return new Vector2D(subtractionResult);
            if (v1.Dimension == 3)
                return new Vector3D(subtractionResult);
            return new Vector(subtractionResult);
        }

        public static Vector operator *(double scalar, Vector v2)
        {
            return new Vector(v2.Data.ScalarMultiply(scalar));
        }

        public double this[int idx]
        {
            get { return Data[idx]; }
            set { Data[idx] = value; }
        }

        public bool Equals(Vector other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Dimension == other.Dimension && Data.SequenceEqual(other.Data);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as Vector;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Dimension * 397) ^ Data.GetHashCode();
            }
        }

        public static bool operator ==(Vector left, Vector right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Vector left, Vector right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Join(" ", Data.Select(x => x.ToString(CultureInfo.InvariantCulture)));
        }
    }
}