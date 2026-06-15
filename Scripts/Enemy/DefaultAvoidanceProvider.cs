using UnityEngine;

public class DefaultAvoidanceProvider : IAvoidanceProvider
{
    private Collider[] _results = new Collider[20];

    public Vector3 CalculateAvoidance(Vector3 currentPosition, Vector3 forwardDirection, float radius, float strength, float weight, float angleDeg, LayerMask targetLayer)
    {
        int count = Physics.OverlapSphereNonAlloc(currentPosition, radius, _results, targetLayer);
        if (count == 0) return Vector3.zero;

        Vector3 avoidance = Vector3.zero;
        for (int i = 0; i < count; i++)
        {
            Transform other = _results[i].transform;
            if (other == null) continue;

            Vector3 dirToOther = other.position - currentPosition;
            float distance = dirToOther.magnitude;
            if (distance > radius || distance < 0.01f) continue;

            // Проверка угла: учитываем только тех, кто впереди
            float angle = Vector3.Angle(forwardDirection, dirToOther.normalized);
            if (angle > angleDeg) continue;

            // Квадратичный вес от расстояния: ближе -> больше влияние
            float weightFactor = 1f - (distance / radius);
            weightFactor = weightFactor * weightFactor; // квадратичное убывание

            Vector3 awayDir = (currentPosition - other.position).normalized; // направление от соседа
            avoidance += awayDir * weightFactor;
        }

        // Умножаем на общую силу и вес уклонения
        return avoidance * strength * weight;
    }
}