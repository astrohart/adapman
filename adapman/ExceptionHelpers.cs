using System;

namespace adapman
{
    /// <summary>
    /// Provides objects and functionality for dealing with exceptions.
    /// </summary>
    public static class ExceptionHelpers
    {
        /// <summary>
        /// Determines the error message given an exception object.  
        /// </summary>
        /// <param name="exceptionObject">Reference to an instance of an exception object.</param>
        /// <returns>Error message if found; the empty string otherwise.</returns>
        /// <remarks>Tries to cast the object instance provided in the <see cref="exceptionObject"/> parameter to
        /// type <see cref="T:System.Exception"/> and return the value of the <see cref="P:System.Exception.Message"/>
        /// property. If this is not able to be done, then the method returns the empty string.</remarks>
        public static string GetMessageFromExceptionObject(object exceptionObject)
        {
            return !(exceptionObject is Exception exception) ? String.Empty : exception.Message;
        }
    }
}