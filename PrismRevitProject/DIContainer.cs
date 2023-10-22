using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismRevitProject
{
    public class DIContainer
    {
        private static readonly DIContainer _instance = new DIContainer();

        private readonly Dictionary<Type, object> _registrations = new Dictionary<Type, object>();

        public static DIContainer Instance => _instance;

        public void Register<T>(T implementation)
        {
            _registrations[typeof(T)] = implementation;
        }

        public T Resolve<T>()
        {
            if (_registrations.TryGetValue(typeof(T), out var implementation))
            {
                return (T)implementation;
            }

            throw new InvalidOperationException($"Type {typeof(T)} has not been registered.");
        }
    }

}
