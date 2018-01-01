using System;
using System.Collections.Generic;

namespace Commons.DataProcessing
{
    public interface IDataSource<out T> : IDisposable
    {
        void Reset();
        IEnumerable<T> GetNext();
    }
}