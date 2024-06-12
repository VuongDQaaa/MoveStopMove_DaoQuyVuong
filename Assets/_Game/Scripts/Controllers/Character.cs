using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum AnimationState { idle, run, attack, dance, die }
    [Header("Animation")]
    [SerializeField] private Animator anim;
    private float originAnimSpeed;
    protected bool isDeath;
    private AnimationState currentAnimationState;

    [Header("Body")]
    [SerializeField] protected SkinnedMeshRenderer bodyColor;
    [SerializeField] protected SkinnedMeshRenderer pantColor;

    [Header("Character Infor")]
    public int currentPoint;
    public string characterName = "Default";
    private int scoreThreshhold = 3;
    private float sizeIncreaseFactor = 1.1f;
    private float rangeIncreaseFactor = 1.1f;
    protected MapController currentMap;

    [Header("Attack")]
    [SerializeField] private LayerMask attackLayer;
    protected Transform currentTarget;
    public bool isAttack = false;

    [Header("Equipment")]
    [SerializeField] protected Weapon currentWeapon;
    [SerializeField] protected Skin currentHat;
    [SerializeField] protected Skin currentPant;
    [SerializeField] protected Skin currentShield;
    [SerializeField] protected Transform weaponHold;
    protected GameObject bulletPrefab;
    public float attackRange;
    [SerializeField] protected float attackSpeed;
    [SerializeField] private Transform root;

    [Header("Skin")]
    [SerializeField] private Transform pant;
    [SerializeField] protected Transform hat;
    [SerializeField] protected Transform shield;

    [Header("Movement")]
    [SerializeField] private LayerMask groundLayer;
    protected bool isMoving = false;

    protected virtual void Awake()
    {
        originAnimSpeed = anim.speed;
        isDeath = false;
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
        bulletPrefab.GetComponent<BulletController>().attacker = transform;
        ObjectPooling.Instance.InstantiatePoolObject(bulletPrefab);
    }

    protected void EquipSkin(Skin skin)
    {
        if (skin.skinType == SkinType.Hat)
        {
            currentHat = skin;
            Instantiate(skin.skinModel, hat.position, hat.rotation, hat);
        }
        else if(skin.skinType == SkinType.Pant)
        {
            currentPant = skin;
            pant.GetComponent<SkinnedMeshRenderer>().material = skin.skinMaterial;
        }
        else if(skin.skinType == SkinType.Shield)
        {
            currentShield = skin;
            Instantiate(skin.skinModel, shield.position, shield.rotation, shield);
        }

        attackRange = attackRange * skin.attackRange;
        attackSpeed = attackSpeed * skin.attackSpeed;
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
            SoundManager.PlaySound(SoundType.SizeUp);
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
        SoundManager.PlaySound(SoundType.Die);
        StartCoroutine(SetDie());
    }

    IEnumerator SetDie()
    {
        isDeath = true;
        ChangeAnim(AnimationState.die);
        yield return new WaitForSeconds(1.5f);
        currentMap.RemoveCharacter(transform.gameObject);
    }
}
