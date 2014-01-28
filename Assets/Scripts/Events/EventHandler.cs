﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EventHandler
{
    public delegate void EventListener<T>(T evt);
    private static Dictionary<System.Type, System.Delegate> _listeners = new Dictionary<System.Type, System.Delegate>();

    public static void register<T>(EventListener<T> listener) where T : struct
    {
        var type = typeof(T);

        if (_listeners.ContainsKey(type))
        {
            _listeners[type] = System.Delegate.Combine(_listeners[type], listener);
        }
        else
        {
            _listeners.Add(type, listener);
        }
    }

    public static void unregister<T>(EventListener<T> listener) where T : struct
    {
        var type = typeof(T);

        if (_listeners.ContainsKey(type))
        {
            var newListeners = System.Delegate.Remove(_listeners[type], listener);

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
        var type = typeof(T);

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