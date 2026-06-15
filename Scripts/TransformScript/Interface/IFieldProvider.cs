using UnityEngine;

public interface IFieldProvider
{
    bool TryGetTransform(out Transform result);
    Transform GetTransform();
}
