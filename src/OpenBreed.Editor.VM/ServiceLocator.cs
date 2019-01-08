using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM
{
    public class ServiceLocator
    {

        #region Private Fields

        private static ServiceLocator _instance;
        private Dictionary<Type, object> _services = new Dictionary<Type, object>();

        #endregion Private Fields

        #region Public Properties

        public static ServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ServiceLocator();

                return _instance;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public T GetService<T>() where T : class
        {
            var serviceType = typeof(T);

            object serviceRef = null;
            if (!_services.TryGetValue(serviceType, out serviceRef))
                throw new InvalidOperationException($"Service with type {serviceType} not registered.");

            return (T)serviceRef;
        }

        public void UnregisterService<T>() where T : class
        {
            var serviceType = typeof(T);

            if (!_services.ContainsKey(serviceType))
                throw new InvalidOperationException($"Service with type {serviceType} not registered.");

            _services.Remove(serviceType);
        }

        public T RegisterService<T>(T serviceRef) where T : class
        {
            var serviceType = typeof(T);

            if (_services.ContainsKey(serviceType))
                throw new InvalidOperationException($"Service with type {serviceType} already registered.");

            _services.Add(typeof(T), serviceRef);
            return serviceRef;
        }

        public bool ServiceExists<T>() where T : class
        {
            return _services.ContainsKey(typeof(T));
        }

        #endregion Public Methods
    }
}
