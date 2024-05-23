using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum AnimationState { idle, run, attack, dance, die }
    [Header("Animation")]
    [SerializeField] private Animator anim;

    [Header("Attack")]
    [SerializeField] private LayerMask attackLayer;
    public float attackRange = 5f;
    [SerializeField] private float attackCoolDown = 0f;
    [SerializeField] protected Transform currentTarget;
    public bool isAttack = false;

    [Header("Weapon")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float attackSpeed = 1f;

    [Header("Movement")]
    [SerializeField] private LayerMask groundLayer;
    public bool isMoving = false;

    private AnimationState currentAnimationState;

    public Vector3 CheckGrounded(Vector3 nextPos)
    {
        RaycastHit hit;
        Debug.DrawRay(nextPos, Vector3.down, Color.blue);
        if (Physics.Raycast(nextPos, Vector3.down, out hit, 2f, groundLayer))
        {
            return hit.point + Vector3.up * 1.2f;
        }
        return transform.position;
    }

    void OnDrawGizmosSelected()
    {
        // Draw detect radius in screen view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected void DetectTarget()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, attackRange, attackLayer);
        if (targetsInViewRadius.Length > 1)
        {
            float targetDistance = Mathf.Infinity;
            foreach (Collider item in targetsInViewRadius)
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);
                if (targetDistance > distance && distance > 0.1f && currentTarget == null)
                {
                    targetDistance = distance;
                    currentTarget = item.transform;
                }
            }
        }
        else
        {
            currentTarget = null;
            attackCoolDown = 0;
        }
    }

    protected void Attack()
    {
        if (!isMoving && currentTarget != null && attackCoolDown <= 0f)
        {
            isAttack = true;
            transform.LookAt(currentTarget.transform.position);
            ChangeAnim(AnimationState.attack);
            GenerateBullet(currentTarget);
            attackCoolDown = attackSpeed;
        }

        if (attackCoolDown > 0f)
        {
            attackCoolDown -= Time.deltaTime;
        }

        if(isAttack == false || attackCoolDown < 0)
        {
            attackCoolDown = 0;
        }
    }

    private void GenerateBullet(Transform target)
    {
        //Create bullet and shoot it with direction from character to target
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector3 dir = target.position - transform.position;
        dir.y = 1f;
        //Use velocity
        newBullet.GetComponent<Rigidbody>().velocity = dir * bulletSpeed;
    }

    private void UpSize()
    { }

    protected void ChangeAnim(AnimationState animationState)
    {
        if (currentAnimationState != animationState)
        {
            anim.ResetTrigger(animationState.ToString());
            currentAnimationState = animationState;
            anim.SetTrigger(currentAnimationState.ToString());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            StartCoroutine(SetDie());
        }
    }

    IEnumerator SetDie()
    {
        ChangeAnim(AnimationState.die);
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
