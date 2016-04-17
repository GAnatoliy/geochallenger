using System;
using System.Runtime.Serialization;


namespace GeoChallenger.Services.Interfaces.Exceptions
{
    /// <summary>
    /// Error that is occurred and usually can't be restored, in most cases it is information for developers.
    /// </summary>
    [Serializable]
    public class InternalErrorException : ServiceException
    {
        public InternalErrorException(string message) : base(message)
        {
        }

        public InternalErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InternalErrorException(Exception innerException) : base("Service internal error is occurred.", innerException)
        {
            
        }

        public InternalErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}