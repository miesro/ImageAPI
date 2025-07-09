using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indg.Services.Exceptions
{
    /// <summary>
    /// Exception thrown when an image file is invalid or cannot be processed.
    /// </summary>
    public class InvalidImageException : Exception
    {
        public InvalidImageException()
        {
        }

        public InvalidImageException(string message)
            : base(message)
        {
        }

        public InvalidImageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
