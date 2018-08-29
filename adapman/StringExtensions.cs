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
        /// True if the collection contains at least one element that contains
        /// the string specified by the value of the <see cref="findWhat" /> parameter,
        /// irregardless of case; false otherwise.
        /// </returns>
        public static bool ContainsNoCase(this ICollection<string> collection, string findWhat)
        {
            // If the specified collection is a null reference or does not
            // contain any elements, then, by definition, it doesn't contain the
            // value being searched for.
            if (collection == null || !collection.Any())
                return false;

            // Perform a case-insensitive search for the value in the collection
            // by first converting each element and the findWhat value to
            // lowercase and then comparing.
            return collection.Any(element => element.ToLowerInvariant()
                .Contains(findWhat.ToLowerInvariant()));
        }
    }
}