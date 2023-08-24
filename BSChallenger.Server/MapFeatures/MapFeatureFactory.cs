using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

namespace BSChallenger.Server.MapFeatures
{
    public static class MapFeatureFactory
    {
        public static List<IMapFeature> CreateInstancesFromCurrentAssembly()
        {
            List<IMapFeature> instances = new List<IMapFeature>();

            // Get the current assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Get all types in the assembly that implement the IMapFeature interface
            var types = assembly.GetTypes().Where(t => typeof(IMapFeature).IsAssignableFrom(t));

            // Create an instance of each type and add it to the list
            foreach (Type type in types)
            {
                IMapFeature instance = (IMapFeature)Activator.CreateInstance(type);
                instances.Add(instance);
            }

            return instances;
        }
    }
}
