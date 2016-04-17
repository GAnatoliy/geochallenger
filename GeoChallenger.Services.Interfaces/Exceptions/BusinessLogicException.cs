using System;
using System.Runtime.Serialization;
using GeoChallenger.Services.Interfaces.Enums;


namespace GeoChallenger.Services.Interfaces.Exceptions
{
    /// <summary>
    /// Business logic error, usually user should be notified about this issue, since he can fix this issue.
    /// </summary>
    [Serializable]
    public class BusinessLogicException : ServiceException
    {
        public ErrorCode ErrorCode { get; private set; }

        public BusinessLogicException(ErrorCode errorCode) : base(FormatMessage(errorCode))
        {
            ErrorCode = errorCode;
        }

        public BusinessLogicException(ErrorCode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        public BusinessLogicException(ErrorCode errorCode, Exception inner) : base(FormatMessage(errorCode), inner)
        {
            ErrorCode = errorCode;
        }

        //TODO: form message from error code and input message
        public BusinessLogicException(ErrorCode errorCode, string message, Exception inner) : base(message, inner)
        {
            ErrorCode = errorCode;
        }

        protected BusinessLogicException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                ErrorCode = (ErrorCode) info.GetValue("ErrorCode", typeof (ErrorCode));
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info != null)
            {
                info.AddValue("ErrorCode", ErrorCode);
            }
        }

        private static string FormatMessage(ErrorCode errorCode)
        {
            return $"Next error is occurred in service layer: {errorCode}";
        }
    }
}