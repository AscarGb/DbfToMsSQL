using System;
using System.Runtime.Serialization;

namespace BdfToMsSQL.Loader
{
    [Serializable]
    internal class CreateReaderException : Exception
    {
        public CreateReaderException()
        {
        }

        public CreateReaderException(string message) : base(message)
        {
        }

        public CreateReaderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CreateReaderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}