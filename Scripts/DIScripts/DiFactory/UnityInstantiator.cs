using System;
using UnityEngine;

public class UnityInstantiator : IInstantiator
{
    public T Instantiate<T>(T prefab, Transform parent = null) where T : MonoBehaviour
    {
        if (prefab == null) throw new ArgumentNullException(nameof(prefab));
        return UnityEngine.Object.Instantiate(prefab, parent);
    }
}
