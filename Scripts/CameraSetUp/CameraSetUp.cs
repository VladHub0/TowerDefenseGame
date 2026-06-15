using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSetUp : MonoBehaviour, IInjects<CameraDeps>
{
    private IFieldProvider fieldProvider;
    [SerializeField, Range(0f, 89f)] private float pitchAngle = 45f; 
    [SerializeField] private bool preferY = true; 
    [SerializeField] private float tolerance = 0.1f;


    public void Inject(CameraDeps dependencies)
    {
        if (dependencies == null) return; 
        if (fieldProvider == null && dependencies.FieldProvider != null) 
        { 
            fieldProvider = dependencies.FieldProvider;
        } 
    }
    void Start()
    {
        if (fieldProvider == null)
        {
            Debug.LogError("Field provider not assigned.");
            return;
        }

        if (!fieldProvider.TryGetTransform(out Transform field))
        {
            Debug.LogError("Main field transform not found. Assign in inspector or add object with tag.");
            return;
        }

        Vector3 center = field.position;
        Vector3 size = GetFieldSize(field); 

        float yOffsetDesired = size.x;     
        float zOffsetDesired = size.z;      

        float pitchRad = pitchAngle * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(0f, Mathf.Sin(pitchRad), -Mathf.Cos(pitchRad)).normalized;

        float dFromY = yOffsetDesired / dir.y;
        float dFromZ = zOffsetDesired / -dir.z;

        float distance;
        if (Mathf.Abs(dFromY - dFromZ) <= tolerance)
        {
            distance = (dFromY + dFromZ) * 0.5f;
        }
        else
        {
            if (preferY)
            {
                distance = dFromY;
                float actualZ = -dir.z * distance;
                Debug.LogWarning($"Requested y={yOffsetDesired} and z={zOffsetDesired} are inconsistent for pitch {pitchAngle}°. Using y as priority. Resulting z offset = {actualZ:F3} (desired {zOffsetDesired}).");
            }
            else
            {
                distance = dFromZ;
                float actualY = dir.y * distance;
                Debug.LogWarning($"Requested y={yOffsetDesired} and z={zOffsetDesired} are inconsistent for pitch {pitchAngle}°. Using z as priority. Resulting y offset = {actualY:F3} (desired {yOffsetDesired}).");
            }
        }

        Vector3 camPos = center + dir * distance;
        transform.position = camPos;
        transform.LookAt(center, Vector3.up);
    }

    private Vector3 GetFieldSize(Transform field)
    {
        Renderer r = field.GetComponentInChildren<Renderer>();
        if (r != null) return r.bounds.size;

        if (field.TryGetComponent<BoxCollider>(out var bc)) return Vector3.Scale(bc.size, field.lossyScale);

        Debug.LogWarning("Field has no Renderer or BoxCollider. Using fallback size (20, 0.1, 20).");
        return new Vector3(20f, 0.1f, 20f);
    }

    
}
