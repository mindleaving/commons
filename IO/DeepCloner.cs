using System.IO;
using System.Runtime.Serialization;

namespace Commons.IO
{
    public static class DeepCloner<T>
    {
        public static T Clone(T obj)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return (T) serializer.ReadObject(stream);
            }
        }
    }
}
