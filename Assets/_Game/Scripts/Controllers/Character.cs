using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum AnimationState { idle, run, attack, dance, die }
    [Header("Animation")]
    [SerializeField] private Animator anim;
    [Header("Attack")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private GameObject currentTarget;
    protected bool isAttack = false;
    [Header("Movement")]
    [SerializeField] private LayerMask groundLayer;
    protected bool isMoving = false;

    private AnimationState currentAnimationState;

    public Vector3 CheckGrounded(Vector3 nextPos)
    {
        RaycastHit hit;
        if (Physics.Raycast(nextPos, Vector3.down, out hit, 2f, groundLayer))
        {
            return hit.point + Vector3.up * 1.2f;
        }
        return transform.position;
    }

    void OnDrawGizmosSelected()
    {
        // Vẽ bán kính phát hiện trong Scene View để dễ dàng điều chỉnh
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected void DetectTarget()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, attackRange, attackLayer);
        if(targetsInViewRadius.Length > 0)
        {
            currentTarget = targetsInViewRadius[0].transform.gameObject;
        }
        else
        {
            currentTarget = null;
        }
    }

    protected void AttackControl()
    {
        if(isMoving == false && isAttack == false && currentTarget != null)
        {
            StartCoroutine(Attack());
        }
    }

    private void GenerateBullet(GameObject target)
    {
        //Create bullet and shoot it with direction from character to target
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector3 dir = target.transform.position - transform.position;
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

    IEnumerator Attack()
    {
        isAttack = true;
        Debug.Log("Attack");
        ChangeAnim(AnimationState.attack);
        yield return new WaitForSeconds(0.5f);
        GenerateBullet(currentTarget);
    }

    IEnumerator SetDie()
    {
        ChangeAnim(AnimationState.die);
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
