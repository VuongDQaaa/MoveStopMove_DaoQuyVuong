using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum AnimationState { idle, run, attack, dance, die }
    [Header("Animation")]
    [SerializeField] private Animator anim;
    private float originAnimSpeed;
    [SerializeField] protected bool isDeath;
    private AnimationState currentAnimationState;

    [Header("Equipments")]
    [SerializeField] protected SkinnedMeshRenderer bodyColor;
    [SerializeField] protected SkinnedMeshRenderer pantColor;

    [Header("Character Infor")]
    public int currentPoint;
    public string characterName = "Default";
    private int scoreThreshhold = 3;
    private float sizeIncreaseFactor = 1.1f;
    private float rangeIncreaseFactor = 1.1f;
    [SerializeField] protected MapController currentMap;

    [Header("Attack")]
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] protected Transform currentTarget;
    public bool isAttack = false;

    [Header("Weapon")]
    private Weapon currentWeapon;
    [SerializeField] private Transform weaponHold;
    [SerializeField] private GameObject bulletPrefab;
    public float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private Transform root;

    [Header("Movement")]
    [SerializeField] private LayerMask groundLayer;
    protected bool isMoving = false;

    protected virtual void Awake()
    {
        originAnimSpeed = anim.speed;
        isDeath = false;
        bulletPrefab.GetComponent<BulletController>().attacker = transform;
        ObjectPooling.Instance.InstantiatePoolObject(bulletPrefab);
        Cache.AddCharacter(transform.GetComponent<Collider>(), transform.GetComponent<Character>());
    }

    public void SetCurrentMap(Transform map)
    {
        currentMap = map.GetComponent<MapController>();
    }

    protected void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        //Spawn weapon on character hand
        Instantiate(currentWeapon.weapmonPrefab, weaponHold.position, weaponHold.rotation, weaponHold);
        //Update bullet prefab
        bulletPrefab = currentWeapon.bulletPrefab;
        //Update character status
        attackRange = attackRange * currentWeapon.attackRange;
        attackSpeed = attackSpeed * currentWeapon.attackSpeed;
    }

    public Vector3 CheckGrounded(Vector3 nextPos)
    {
        //check if the below surface is walkable by using layerMash
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

    //Detect character in attack range
    protected void DetectTarget()
    {
        //use Overlap to detect all collider in attack range
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, attackRange, attackLayer);
        //get nearest target
        if (targetsInViewRadius.Length > 1)
        {
            float targetDistance = Mathf.Infinity;
            foreach (Collider item in targetsInViewRadius)
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);
                if (targetDistance > distance
                    && item.transform.GetComponent<Character>() != Cache.GetCharacter(transform.GetComponent<Collider>()))
                {
                    targetDistance = distance;
                    currentTarget = item.transform;
                }
            }
        }
        else
        {
            currentTarget = null;
        }
    }

    //Auto attack character if not moving
    protected void Attack()
    {
        if (!isMoving && currentTarget != null)
        {
            isAttack = true;
            //Update character postion when attacking
            transform.LookAt(currentTarget.transform.position);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            //Change animation
            ChangeAnim(AnimationState.attack);
        }
        else
        {
            weaponHold.gameObject.SetActive(true);
        }

        if (!isDeath)
        {
            UpdateAttackAnimSpeed();
        }
    }

    public void AttackEvent()
    {
        if (currentTarget != null)
        {
            GenerateBullet(currentTarget);
        }
    }

    //Update attack animation follow attack speed
    private void UpdateAttackAnimSpeed()
    {
        if (currentAnimationState == AnimationState.attack)
        {
            anim.speed = attackSpeed;
        }
        else
        {
            anim.speed = originAnimSpeed;
        }
    }

    //Get bullet from pool
    private void GenerateBullet(Transform target)
    {
        GameObject bullet = ObjectPooling.Instance.GetPoolObjectByAttacker(transform, root);
        if (bullet != null)
        {
            bullet.GetComponent<BulletController>().SetTargetPos(target.position);
        }
    }

    //Up size characte when current score > limit
    protected void UpSize()
    {
        if (currentPoint >= scoreThreshhold)
        {
            transform.localScale *= sizeIncreaseFactor;
            attackRange = attackRange * rangeIncreaseFactor;
            scoreThreshhold += scoreThreshhold;
        }
    }

    //change score of bullet hit target
    public void AddScore(int victimScore)
    {
        if (victimScore <= currentPoint)
        {
            currentPoint++;
        }
        else
        {
            currentPoint += victimScore;
        }
        UpSize();
    }

    public bool IsDeath()
    {
        return isDeath;
    }

    public Color GetCharacterColor()
    {
        return bodyColor.material.color;
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

    public void OnDie()
    {
        StartCoroutine(SetDie());
    }

    IEnumerator SetDie()
    {
        isDeath = true;
        ObjectPooling.Instance.DetroyBulletByAttacker(transform);
        ChangeAnim(AnimationState.die);
        yield return new WaitForSeconds(1.5f);
        currentMap.RemoveCharacter(transform.gameObject);
    }
}
