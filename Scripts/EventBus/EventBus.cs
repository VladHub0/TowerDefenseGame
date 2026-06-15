using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EventBus : IEventBus
    {
        private Dictionary<string, List<CallbackWithPriority>> _signalCallbacks = new();

        public void Subscribe<T>(Action<T> callback, int priority = 0)
        {
            string key = typeof(T).Name;
            if (!_signalCallbacks.ContainsKey(key))
                _signalCallbacks[key] = new List<CallbackWithPriority>();

            _signalCallbacks[key].Add(new CallbackWithPriority(priority, callback));
            _signalCallbacks[key] = _signalCallbacks[key].OrderByDescending(x => x.Priority).ToList();
        }

        public void Unsubscribe<T>(Action<T> callback)
        {
            string key = typeof(T).Name;
            if (_signalCallbacks.TryGetValue(key, out var list))
            {
                var item = list.FirstOrDefault(x => x.Callback.Equals(callback));
                if (item != null)
                    list.Remove(item);
            }
            else
            {
                Debug.LogError($"Trying to unsubscribe for non-existing key: {key}");
            }
        }

        public void Invoke<T>(T signal)
        {
            string key = typeof(T).Name;
            if (_signalCallbacks.TryGetValue(key, out var list))
            {
                foreach (var item in list)
                {
                    var callback = item.Callback as Action<T>;
                    callback?.Invoke(signal);
                }
            }
        }
    }