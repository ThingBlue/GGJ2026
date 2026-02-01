using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Inspector members

    public float panSmoothTime;

    public float boundaryStart;
    public float boundaryEnd;

    #endregion

    private float targetPosition;
    private float panVelocity;

    public static CameraController instance;
    private void Awake()
    {
        // Singleton
        if (instance == null) instance = this;
        else Destroy(this);

        targetPosition = transform.position.y;
    }

    private void LateUpdate()
    {
        targetPosition = PlayerController.instance.transform.position.y; // Stay on player

        // Move towards target position
        Vector3 newPosition = transform.position;
        newPosition.y = Mathf.SmoothDamp(newPosition.y, targetPosition, ref panVelocity, panSmoothTime);
        newPosition.y = Mathf.Clamp(newPosition.y, boundaryStart, boundaryEnd); // Clamp to boundary
        transform.position = newPosition;
    }
}
