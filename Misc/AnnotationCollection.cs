using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Commons.Misc
{
    [DataContract]
    public class AnnotationCollection
    {
        [DataMember]
        public List<Annotation> Annotations { get; set; } = new List<Annotation>();
    }
}