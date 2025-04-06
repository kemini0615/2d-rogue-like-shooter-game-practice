using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector2 cameraRange;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("No target object to follow.");
            return;
        }

        Vector3 cameraPosition = target.position + new Vector3(0, 0, -10f);

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, -cameraRange.x, +cameraRange.x);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, -cameraRange.y, +cameraRange.y);

        transform.position = cameraPosition;
    }
}
