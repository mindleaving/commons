using System.IO;
using System.Runtime.Serialization;

namespace Commons.IO
{
    public class Serializer<T>
    {
        private readonly DataContractSerializer serializer;

        public Serializer()
        {
            serializer = new DataContractSerializer(typeof(T));
        }

        public T Load(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                return Load(stream);
            }
        }

        public T Load(Stream stream)
        {
            return (T)serializer.ReadObject(stream);
        }

        public void Store(T obj, string path)
        {
            using (var stream = File.Create(path))
            {
                Store(obj, stream);
            }
        }

        public void Store(T obj, Stream stream)
        {
            serializer.WriteObject(stream, obj);
        }
    }
}