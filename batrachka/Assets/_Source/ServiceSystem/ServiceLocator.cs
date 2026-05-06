using System;
using System.Collections.Generic;
using ViewSystem;

namespace ServiceSystem
{
    public class ServiceLocator : IService
    {
        private Dictionary<Type, object> services = new Dictionary<Type, object>();

        public ServiceLocator(IFadeService fadeService, ISoundPlayer soundPlayer)
        {
            services[typeof(IFadeService)] = fadeService;
            services[typeof(ISoundPlayer)] = soundPlayer;
        }

        public T GetService<T>()
        {
            return (T)services[typeof(T)];
        }
    }
}