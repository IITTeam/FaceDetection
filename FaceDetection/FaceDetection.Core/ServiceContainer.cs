using System;
using System.Collections.Generic;

namespace FaceDetection.Core
{
    public class ServiceContainer
    {
        private readonly static Dictionary<Type, Func<object>> services = new Dictionary<Type, Func<object>>();
        public void RegisterService<T>(Func<T> factory)
            where T : class
        {
            services[typeof(T)] = () => factory();
        }

        internal static object GetService(Type type)
        {
            return services[type]();
        }
    }
}
