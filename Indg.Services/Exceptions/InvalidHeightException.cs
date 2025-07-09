using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indg.Services.Exceptions
{

    /// <summary>
    /// Exception thrown when an image height is invalid, such as when it exceeds the original image height.
    /// </summary>
    public class InvalidHeightException : Exception
    {
        public InvalidHeightException()
        {
        }

        public InvalidHeightException(string message)
            : base(message)
        {
        }

        public InvalidHeightException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
