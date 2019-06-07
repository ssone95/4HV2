//
// _4H_Application.Models.Horses.Exceptions.HorseNotFoundException.cs: Exception
//   that is thrown when a horse is attempted to be obtained but does not exist.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using System;

namespace _4H_Application.Models.Horses.Exceptions {

    /// <summary>
    /// Exception that indicates a horse was not found.
    /// </summary>
    public class HorseNotFoundException : Exception {

        /// <summary>
        /// Constructor for the exception.
        /// </summary>
        public HorseNotFoundException() {
        }

        /// <summary>
        /// Constructor for the exception.
        /// </summary>
        /// <param name="message">The message that accompanies the exception</param>
        public HorseNotFoundException(string message)
            : base(message) {
        }

        /// <summary>
        /// Constructor for the exception.
        /// </summary>
        /// <param name="message">The message that accompanies the exception</param>
        /// <param name="inner">The internal exception.</param>
        public HorseNotFoundException(string message, Exception inner)
            : base(message) {
        }

    }

}
