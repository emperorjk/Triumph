using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Main
{
    public class ResourceCache
    {
        private static List<Object> resourcesStatic = new List<Object>();
        private static bool hasCachedResources;

        public static void CacheResources()
        {
            if(!hasCachedResources)
            {
                hasCachedResources = true;
                Resources.LoadAll("JSON/Levels/").ToList().ForEach(x => resourcesStatic.Add(x));
            }
        }

        public static Object GetResource(string name)
        {
            return resourcesStatic.SingleOrDefault(x => x.name.Equals(name));
        }

        public static T GetResource<T>(string name) where T : Object
        {
            return (T)resourcesStatic.SingleOrDefault(x => x.name.Equals(name));
        }
    }
}
