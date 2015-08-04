using System;
using System.Runtime.Serialization;

namespace FaceDetection.Core
{
    public class NoHumanException: Exception
    {
        public NoHumanException() { }

        public NoHumanException(string message) : base(message) { }

        public NoHumanException(string message, Exception inner) : base(message, inner) { }

        protected NoHumanException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
