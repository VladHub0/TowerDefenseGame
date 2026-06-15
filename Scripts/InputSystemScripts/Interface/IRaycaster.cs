using UnityEngine;

public interface IRaycaster { 
    bool RaycastFromScreen(Vector2 screenPos, out RaycastHit hit, float maxDistance = 1000f, int layerMask = ~0); 
}