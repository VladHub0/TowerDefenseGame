using UnityEngine;

public interface IMover
{
    void MoveTowards(Transform transform, Vector3 target, float speed, float turnSpeed);
    void MoveInDirection(Transform transform, Vector3 direction, float speed, float turnSpeed);
    bool ReachedTarget(Transform transform, Vector3 target, float threshold = 0.8f);
}