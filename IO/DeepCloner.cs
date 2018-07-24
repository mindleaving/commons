using Newtonsoft.Json;

namespace Commons.IO
{
    public static class DeepCloner<T>
    {
        public static T Clone(T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
