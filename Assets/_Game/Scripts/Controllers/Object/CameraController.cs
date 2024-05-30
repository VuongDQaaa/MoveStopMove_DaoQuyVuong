using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform currentTarget;
    //[SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;

    void Update()
    {
        if (currentTarget != null)
        {
            Vector3 desiredPosition = currentTarget.position + offset;
            //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = desiredPosition;

            transform.LookAt(currentTarget.position);
        }
    }

    public void SetTarGet(GameObject target)
    {
        currentTarget = target.transform;
    }

    public void DeleteTarget()
    {
        currentTarget = null;
    }
}

