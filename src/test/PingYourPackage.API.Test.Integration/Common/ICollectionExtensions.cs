using System.Collections.Generic;

namespace System.Collections.Generic {
    
    internal static class ICollectionExtensions {

        internal static void AddTo<T>(
            this ICollection<T> destination,
            IEnumerable<T> source) {

            foreach (T item in source) {
                destination.Add(item);
            }
        }
    }
}