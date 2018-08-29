using System.Collections.Generic;
using System.Linq;

namespace adapman
{
    /// <summary>
    /// Provides extension methods for strings and string collections
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether the collection contains any values, ignoring case.
        /// </summary>
        /// <param name="collection">
        /// Reference to a collection of strings that is to be searched.
        /// </param>
        /// <param name="findWhat">
        /// Value that is to be searched for.
        /// </param>
        /// <returns>
        /// True if the collection contains the <see cref="findWhat" /> value,
        /// irregardless of case; false otherwise.
        /// </returns>
        public static bool ContainsNoCase(this ICollection<string> collection, string findWhat)
        {
            if (collection == null || !collection.Any())
                return false;

            return collection.Any(element => element.ToLowerInvariant().Contains(findWhat.ToLowerInvariant()));
        }
    }
}