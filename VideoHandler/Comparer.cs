using Database.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace VideoHandler
{
    class Comparer : IEqualityComparer<Video>
    {
        public bool Equals(Video x, Video y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] Video obj)
        {
            return obj.Id;
        }
    }
}
