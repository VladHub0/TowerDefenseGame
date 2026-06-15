using UnityEngine;

public class GroundPositionProvider : MonoBehaviour, IPositionProvider
{
    [SerializeField] private string _groundTag = "MainField";
    [SerializeField] private float _raycastDistance = 1000f;
    [SerializeField] private LayerMask _layerMask = Physics.DefaultRaycastLayers;

    public Vector3 GetPlacementPosition(GameObject place, GameObject towerPrefab)
    {
        Vector3 startPos = place.transform.position;
        Ray ray = new Ray(startPos + Vector3.up * _raycastDistance, Vector3.down);
        RaycastHit[] hits = Physics.RaycastAll(ray, _raycastDistance * 2, _layerMask, QueryTriggerInteraction.Ignore);

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag(_groundTag))
            {
                float offset = GetBottomOffset(towerPrefab);
                return hit.point + Vector3.up * offset;
            }
        }

        Debug.LogWarning($"Ground with tag '{_groundTag}' not found for place {place.name}. Using place position.");
        return startPos;
    }

    private float GetBottomOffset(GameObject towerPrefab)
    {

        Renderer rend = towerPrefab.GetComponentInChildren<Renderer>();
        if (rend != null)
            return rend.bounds.extents.y;

        return 0f;
    }
}