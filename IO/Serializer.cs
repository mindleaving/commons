using System.IO;
using Newtonsoft.Json;

namespace Commons.IO
{
    public class Serializer<T>
    {
        private readonly JsonSerializer serializer;

        public Serializer()
        {
            serializer = new JsonSerializer();
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
            var streamReader = new StreamReader(stream);
            return serializer.Deserialize<T>(new JsonTextReader(streamReader));
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
            var streamWriter = new StreamWriter(stream);
            serializer.Serialize(streamWriter, obj);
            streamWriter.Flush();
        }
    }
}