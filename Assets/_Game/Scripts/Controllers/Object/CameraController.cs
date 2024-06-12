using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private Transform currentTarget;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 startOffset;
    [SerializeField] private Vector3 skinOffset;

    [SerializeField] private Vector3 originOffset;
    private void Awake()
    {
        originOffset = offset;
    }

    void LateUpdate()
    {
        if (currentTarget != null)
        {
            if (GameManager.Instance.currentGameState == GameState.Start)
            {
                CameraStartMode();
            }
            else if (GameManager.Instance.currentGameState == GameState.Shopping)
            {
                CameraShopMode();
            }
            else if (GameManager.Instance.currentGameState == GameState.Playing)
            {
                Vector3 desiredPosition = currentTarget.position + offset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
                transform.position = smoothedPosition;
                transform.LookAt(currentTarget.position);
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

    private void CameraShopMode()
    {
        //move camera when open skin store UI
        Vector3 camPos = currentTarget.position + skinOffset;
        transform.position = camPos;
    }

    public void SetCamTarget(GameObject target)
    {
        //Update target for camera
        currentTarget = target.transform;
    }

    public void ChangeOffSet()
    {
        //Change offset when player bigger
        offset.y += 2;
        offset.z -= 2f;
    }

    public void Reset()
    {
        offset = originOffset;
    }
}

