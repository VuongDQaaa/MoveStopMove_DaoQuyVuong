using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Transform attacker;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float rotateSpeed;
    private Vector3 targetPos = Vector3.zero;

    void Update()
    {
        if (targetPos != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, bulletSpeed * Time.deltaTime);
            transform.Rotate(0, 0, -rotateSpeed);

            if(Vector3.Distance(transform.position, targetPos) <= 0.1f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void SetTargetPos(Vector3 newPos)
    {
        targetPos = newPos;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform != attacker)
        {
            gameObject.SetActive(false);
        }
    }
}
