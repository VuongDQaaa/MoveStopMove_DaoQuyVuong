using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private Transform currentTarget;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 startOffset;

    void LateUpdate()
    {
        if (currentTarget != null)
        {
            if (GameManager.Instance.currentGameState != GameState.Start)
            {
                Vector3 desiredPosition = currentTarget.position + offset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
                transform.position = smoothedPosition;

                transform.LookAt(currentTarget.position);
            }
            else
            {
                CameraStartMode();
            }
        }
    }

    private void CameraStartMode()
    {
        //move camera near player when game start
        Vector3 camPos = currentTarget.position + startOffset;
        transform.position = camPos;
        transform.LookAt(currentTarget.position);
    }

    public void SetCamTarget(GameObject target)
    {
        currentTarget = target.transform;
    }

    public void ChangeOffSet()
    {
        offset.y += 2;
        offset.z -= 2f;
    }
}

