using System;
using System.Collections.Generic;
using UISystem;
using ViewSystem;

namespace ServiceSystem
{
    public class ServiceLocator : IService
    {
        private Dictionary<Type, object> services =
            new Dictionary<Type, object>();

        public ServiceLocator(
            IFadeService fadeService,
            ISoundPlayer soundPlayer,
            ISaver saver,
            Score score)
        {
            services[typeof(IFadeService)] = fadeService;

            services[typeof(ISoundPlayer)] = soundPlayer;

            services[typeof(ISaver)] = saver;

            services[typeof(Score)] = score;
        }

        public T GetService<T>()
        {
            return (T)services[typeof(T)];
        }
    }
}
