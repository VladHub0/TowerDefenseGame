using UnityEngine;

public interface IInstantiator
{
    T Instantiate<T>(T prefab, Transform parent = null) where T : MonoBehaviour;
}
    