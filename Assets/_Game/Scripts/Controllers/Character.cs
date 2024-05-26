using System.Collections;
using System.Data.Common;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum AnimationState { idle, run, attack, dance, die }
    [Header("Animation")]
    [SerializeField] private Animator anim;
    [SerializeField] protected bool isDeath;
    private AnimationState currentAnimationState;

    [Header("Current Point")]
    public int currentPoint = 1;
    private int scoreThreshhold = 10;
    private float sizeIncreaseFactor = 1.1f;

    [Header("Attack")]
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private float attackCoolDown = 0f;
    [SerializeField] protected Transform currentTarget;
    public bool isAttack = false;

    [Header("Weapon")]
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private Transform weaponHold;
    [SerializeField] private GameObject bulletPrefab;
    public float attackRange = 5f;
    [SerializeField] private float attackSpeed = 1.7f;
    [SerializeField] private Transform root;

    [Header("Movement")]
    [SerializeField] private LayerMask groundLayer;
    public bool isMoving = false;

    void Awake()
    {
        isDeath = false;
        if (currentWeapon != null)
        {
            EquipWeapon(currentWeapon);
        }
        bulletPrefab.GetComponent<BulletController>().attacker = transform;
        ObjectPooling.Instance.InstantiatePoolObject(bulletPrefab);
    }

    private void EquipWeapon(Weapon weapon)
    {
        //Spawn weapon on character hand
        Instantiate(weapon.weapmonPrefab, weaponHold.position, weaponHold.rotation, weaponHold);
        //Update bullet prefab
        bulletPrefab = weapon.bulletPrefab;
        //Update character status
        attackRange += weapon.attackRange;
        attackSpeed -= weapon.attackSpeed;
    }

    public Vector3 CheckGrounded(Vector3 nextPos)
    {
        RaycastHit hit;
        Debug.DrawRay(nextPos, Vector3.down, Color.blue);
        if (Physics.Raycast(nextPos, Vector3.down, out hit, groundLayer))
        {
            Vector3 newPos = hit.point + Vector3.up * transform.localScale.y;
            newPos.y += 0.2f;
            return newPos;
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

        if (isAttack == false || attackCoolDown < 0)
        {
            attackCoolDown = 0;
        }
    }

    private void GenerateBullet(Transform target)
    {
        GameObject bullet = ObjectPooling.Instance.GetPoolObjectByAttacker(transform, root);
        bullet.GetComponent<BulletController>().SetTargetPos(target.position);
    }

    protected void UpSize()
    {
        if(currentPoint >= scoreThreshhold)
        {
            transform.localScale *= sizeIncreaseFactor;
            scoreThreshhold += scoreThreshhold;
        }
    }

    public void AddScore(int victimScore)
    {
        if (victimScore <= currentPoint)
        {
            currentPoint++;
        }
        else
        {
            currentPoint += victimScore - currentPoint;
        }
        UpSize();
    }

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
            if (other.GetComponent<BulletController>().attacker != transform)
            {
                StartCoroutine(SetDie());
            }
        }
    }

    IEnumerator SetDie()
    {
        isDeath = true;
        ObjectPooling.Instance.DetroyBulletByAttacker(transform);
        ChangeAnim(AnimationState.die);
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.RemoveCharacter(gameObject);
    }
}
