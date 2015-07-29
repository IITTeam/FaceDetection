using System;
using System.Collections.Generic;

namespace FaceDetection.Core
{
    public class ServiceContainer
    {
        private readonly static Dictionary<Type, object> Services = new Dictionary<Type, object>();
        public void RegisterService<T>(T obj)
            where T : class
        {
            Services.Add(typeof(T), obj);
        }

        internal static object GetService(Type type)
        {
            return Services[type];
        }
    }
}
