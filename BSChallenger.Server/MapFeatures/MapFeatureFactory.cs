using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

namespace BSChallenger.Server.MapFeatures
{
    public static class MapFeatureFactory
    {
        private static List<IMapFeature> _features;
        public static List<IMapFeature> CreateInstancesFromCurrentAssembly()
        {
            if (_features == null)
            {
                List<IMapFeature> instances = new();

                // Get the current assembly
                Assembly assembly = Assembly.GetExecutingAssembly();

                var featureType = typeof(IMapFeature);

                // Get all types in the assembly that implement the IMapFeature interface
                var types = assembly.GetTypes().Where(t => featureType.IsAssignableFrom(t) && t != featureType);

                // Create an instance of each type and add it to the list
                foreach (Type type in types)
                {
                    IMapFeature instance = (IMapFeature)Activator.CreateInstance(type);
                    instances.Add(instance);
                }
                _features = instances;
            }

			return _features;
        }
    }
}
