using System;
using UnityEngine;

public class CameraRaycaster : IRaycaster
{
    private readonly Camera _camera;
    public CameraRaycaster(Camera camera) => _camera = camera ?? throw new ArgumentNullException(nameof(camera));

    public bool RaycastFromScreen(Vector2 screenPos, out RaycastHit hit, float maxDistance = 1000f, int layerMask = ~0)
    {
        var ray = _camera.ScreenPointToRay(screenPos);
        return Physics.Raycast(ray, out hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore);
    }
}
