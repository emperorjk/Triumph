using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Main
{
    public class ResourceCache
    {
        private static List<UnityEngine.Object> resourcesStatic = new List<UnityEngine.Object>();
        private static Boolean hasCachedResources = false;

        public static void CacheResources()
        {
            if(!hasCachedResources)
            {
                Debug.Log("Started caching the resources into memory.");
                hasCachedResources = true;
                UnityEngine.Object[] ar = Resources.LoadAll("JSON/Levels/");
                foreach (UnityEngine.Object item in ar)
                {
                    resourcesStatic.Add(item);
                }
                Debug.Log("Done caching the resources into memory.");
            }
        }

        public static UnityEngine.Object getResource(string name)
        {
            return resourcesStatic.SingleOrDefault(x => x.name.Equals(name));
        }

        public static T getResource<T>(string name) where T : UnityEngine.Object
        {
            return (T)resourcesStatic.SingleOrDefault(x => x.name.Equals(name));
        }
    }
}
