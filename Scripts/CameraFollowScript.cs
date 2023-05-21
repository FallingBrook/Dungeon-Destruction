using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;
    }
    private void Update()
    {
        //Application.targetFrameRate = 30;
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
