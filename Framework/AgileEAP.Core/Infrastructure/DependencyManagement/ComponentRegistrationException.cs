using System;
using System.Runtime.Serialization;

namespace AgileEAP.Core.Infrastructure.DependencyManagement
{
    [Serializable]
    public class ComponentRegistrationException : AgileEAP.Core.ExceptionHandler.EAPException
    {
        public ComponentRegistrationException(string serviceName)
            : base(String.Format("Component {0} could not be found but is registered in the AgileEAP/engine/components section", serviceName))
        {
        }

        protected ComponentRegistrationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
