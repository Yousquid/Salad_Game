using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Transform followTarget, lookTarget;
    public float followSpeed = 10f;

    private void LateUpdate()
    {
        Vector3 targetPosition = followTarget.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        transform.LookAt(lookTarget);
    }
}
