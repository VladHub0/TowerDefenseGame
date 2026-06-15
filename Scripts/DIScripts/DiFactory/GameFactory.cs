using System;
using UnityEngine;

public class GameFactory<T> : IFactory<T> where T : MonoBehaviour
{
    private readonly DiContainer _container;
    private readonly IInstantiator _instantiator;

    public GameFactory(DiContainer container, IInstantiator instantiator)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
        _instantiator = instantiator ?? throw new ArgumentNullException(nameof(instantiator));
    }

    public T Create(T prefab, Transform parent = null)
    {
        if (prefab == null) throw new ArgumentNullException(nameof(prefab));

        var instance = _instantiator.Instantiate(prefab, parent);
        _container.InjectTo(instance);
        return instance;
    }
}
