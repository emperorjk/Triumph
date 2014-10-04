using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public class EventHandler
    {
        public delegate void EventListener<T>(T evt);

        private static Dictionary<Type, Delegate> _listeners =
            new Dictionary<Type, Delegate>();

        public static void register<T>(EventListener<T> listener) where T : struct
        {
            var type = typeof (T);

            if (_listeners.ContainsKey(type))
            {
                _listeners[type] = Delegate.Combine(_listeners[type], listener);
            }
            else
            {
                _listeners.Add(type, listener);
            }
        }

        public static void unregister<T>(EventListener<T> listener) where T : struct
        {
            var type = typeof (T);

            if (_listeners.ContainsKey(type))
            {
                var newListeners = Delegate.Remove(_listeners[type], listener);

                if (newListeners == null)
                {
                    _listeners.Remove(type);
                }
                else
                {
                    _listeners[type] = newListeners;
                }
            }
        }

        public static void dispatch<T>(T evt) where T : struct
        {
            var type = typeof (T);

            if (_listeners.ContainsKey(type))
            {
                foreach (EventListener<T> dg in _listeners[type].GetInvocationList())
                {
                    try
                    {
                        dg(evt);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                }
            }
        }
    }
}