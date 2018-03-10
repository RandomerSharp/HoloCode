using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTagalang : MonoBehaviour
{
    [Tooltip("Sphere radius.")]
    public float SphereRadius = 1.0f;

    [Tooltip("How fast the object will move to the target position.")]
    public float MoveSpeed = 2.0f;

    /// <summary>
    /// When moving, use unscaled time. This is useful for games that have a pause mechanism or otherwise adjust the game timescale.
    /// </summary>
    [SerializeField]
    [Tooltip("When moving, use unscaled time. This is useful for games that have a pause mechanism or otherwise adjust the game timescale.")]
    private bool useUnscaledTime = true;

    /// <summary>
    /// Used to initialize the initial position of the SphereBasedTagalong before being hidden on LateUpdate.
    /// </summary>
    [SerializeField]
    [Tooltip("Used to initialize the initial position of the SphereBasedTagalong before being hidden on LateUpdate.")]
    private bool hideOnStart;

    [SerializeField]
    [Tooltip("Display the sphere in red wireframe for debugging purposes.")]
    private bool debugDisplaySphere;

    [SerializeField]
    [Tooltip("Display a small green cube where the target position is.")]
    private bool debugDisplayTargetPosition;

    private Vector3 targetPosition;
    private Vector3 optimalPosition;
    private float initialDistanceToCamera;

    private GameObject targetCamera;

    private void Start()
    {
        targetCamera = GameObject.Find("MixedRealityCamera");
        initialDistanceToCamera = Vector3.Distance(transform.position, targetCamera.transform.position);
    }

    private void Update()
    {
        optimalPosition = targetCamera.transform.position + targetCamera.transform.forward * initialDistanceToCamera;
        Vector3 offsetDir = transform.position - optimalPosition;

        if (offsetDir.magnitude > SphereRadius)
        {
            targetPosition = optimalPosition + offsetDir.normalized * SphereRadius;

            float deltaTime = useUnscaledTime
                ? Time.unscaledDeltaTime
                : Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, targetPosition, MoveSpeed * deltaTime);
        }
    }

    private void LateUpdate()
    {
        if (hideOnStart)
        {
            hideOnStart = !hideOnStart;
            gameObject.SetActive(false);
        }
    }

    public void OnDrawGizmos()
    {
        if (Application.isPlaying == false) { return; }

        Color oldColor = Gizmos.color;

        if (debugDisplaySphere)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(optimalPosition, SphereRadius);
        }

        if (debugDisplayTargetPosition)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(targetPosition, new Vector3(0.1f, 0.1f, 0.1f));
        }

        Gizmos.color = oldColor;
    }
}
