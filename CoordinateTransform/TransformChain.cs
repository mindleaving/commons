using System.Collections.Generic;

namespace Commons.CoordinateTransform
{
    public class TransformChain<TIn, TOut> : ICoordinateTransform<TIn, TOut>
    {
        private readonly List<ICoordinateTransform<object, object>> transforms = new List<ICoordinateTransform<object, object>>();

        public TransformChain(params ICoordinateTransform<object, object>[] transforms)
        {
            this.transforms.AddRange(transforms);
        }

        public TOut Transform(TIn point)
        {
            object currentPoint = point;
            for (int transformIdx = 0; transformIdx < transforms.Count; transformIdx++)
            {
                var transform = transforms[transformIdx];
                currentPoint = transform.Transform(currentPoint);
            }
            return (TOut) currentPoint;
        }

        public TIn InverseTransform(TOut point)
        {
            object currentPoint = point;
            for (int transformIdx = transforms.Count - 1; transformIdx >= 0; transformIdx--)
            {
                var transform = transforms[transformIdx];
                currentPoint = transform.InverseTransform(currentPoint);
            }
            return (TIn) currentPoint;
        }
    }
}
