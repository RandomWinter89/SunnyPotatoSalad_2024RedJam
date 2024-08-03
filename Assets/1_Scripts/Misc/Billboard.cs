using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;
    public float angleThreshold = 45f; // Maximum allowed angle in degrees


    private void Awake()
    {
        cam = Camera.main; // Cache the main camera
    }

    private void LateUpdate()
    {
        if (cam == null) return;

        Vector3 direction = cam.transform.position - transform.position;
        direction.y = 0; // Keep the y direction unchanged

        Quaternion rotation = Quaternion.LookRotation(direction);
        Vector3 eulerAngles = rotation.eulerAngles;

        // restrict how tilted the x-axis can be (90 degrees is upright)
        eulerAngles = new Vector3(Mathf.Clamp(eulerAngles.x, 0 - 45, 0 + 45), eulerAngles.y, 0f);


        transform.rotation = Quaternion.Euler(eulerAngles);
    }
}
