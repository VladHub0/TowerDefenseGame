using UnityEngine;

public interface IAvoidanceProvider
{
    Vector3 CalculateAvoidance(Vector3 currentPosition, Vector3 forwardDirection, float radius, float strength, float weight, float angleDeg, LayerMask targetLayer);
}