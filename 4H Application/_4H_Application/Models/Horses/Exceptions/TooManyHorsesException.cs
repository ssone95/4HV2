//
// _4H_Application.Models.Horses.Exceptions.TooManyHorsesException.cs: Exception
//   that is thrown when a horse is attempted to be created when there are the
//   max number of horses already.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using System;

namespace _4H_Application.Models.Horses.Exceptions {

    /// <summary>
    /// Exception that indicates there are too many horses.
    /// </summary>
    public class TooManyHorsesException : Exception {

        /// <summary>
        /// Constructor for the exception.
        /// </summary>
        public TooManyHorsesException() {
        }

        /// <summary>
        /// Constructor for the exception.
        /// </summary>
        /// <param name="message">The message that accompanies the exception</param>
        public TooManyHorsesException(string message)
            : base(message) {
        }

        /// <summary>
        /// Constructor for the exception.
        /// </summary>
        /// <param name="message">The message that accompanies the exception</param>
        /// <param name="inner">The internal exception.</param>
        public TooManyHorsesException(string message, Exception inner) 
            : base(message) {
        }

    }

}
