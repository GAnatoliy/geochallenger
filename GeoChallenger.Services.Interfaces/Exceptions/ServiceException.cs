using System;
using System.Runtime.Serialization;


namespace GeoChallenger.Services.Interfaces.Exceptions
{
    /// <summary>
    /// Main class for exceptions in the services
    /// </summary>
    public abstract class ServiceException: Exception
    {
        public ServiceException(string message) : base(message)
        {
            
        }

        public ServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }

        public ServiceException(SerializationInfo info, StreamingContext context) : base (info, context)
        {
            
        }


    }
}
