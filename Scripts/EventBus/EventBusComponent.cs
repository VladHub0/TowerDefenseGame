using System;
using UnityEngine;

public class EventBusComponent : MonoBehaviour, IEventBus
{
    private EventBus _eventBus;

    private void Initialize()
    {
        if (_eventBus == null)
            _eventBus = new EventBus();
    }

    public void Subscribe<T>(Action<T> callback, int priority = 0)
    {
        Initialize();
        _eventBus.Subscribe(callback, priority);
    }

    public void Unsubscribe<T>(Action<T> callback)
    {
        Initialize();
        _eventBus.Unsubscribe(callback);
    }

    public void Invoke<T>(T signal)
    {
        Initialize();
        _eventBus.Invoke(signal);
    }
}