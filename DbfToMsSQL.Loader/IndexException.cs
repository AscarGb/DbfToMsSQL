using System;
using System.Runtime.Serialization;

namespace DbfToMsSQL.Loader
{
    [Serializable]
    internal class IndexException : Exception
    {
        public IndexException()
        {
        }

        public IndexException(string message) : base(message)
        {
        }

        public IndexException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IndexException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}