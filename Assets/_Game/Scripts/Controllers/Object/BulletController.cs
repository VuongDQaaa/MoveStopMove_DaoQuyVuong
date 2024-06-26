using UnityEngine;

public enum BulletType
{
    Spin = 0,
    Strait = 1,
    Boomerang = 2
}

public class BulletController : MonoBehaviour
{
    public Transform attacker;
    private Transform victim;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private BulletType bulletType;
    private Vector3 targetPos = Vector3.zero;
    private bool isReturned;

    private void OnEnable()
    {
        isReturned = false;
    }

    private void Update()
    {
        BulletControl();
    }

    public void BulletControl()
    {
        if (targetPos != Vector3.zero)
        {
            if (bulletType == BulletType.Spin || bulletType == BulletType.Strait)
            {
                //Bullet movement
                transform.position = Vector3.MoveTowards(transform.position, targetPos, bulletSpeed * Time.deltaTime);
                //Disable bullet if out of attack range
                if (Vector3.Distance(transform.position, targetPos) <= 0.1f)
                {
                    gameObject.SetActive(false);
                }
            }
            else if (bulletType == BulletType.Boomerang)
            {
                BoomerangBullet();
            }

            //Bullet rotate
            if (bulletType == BulletType.Spin || bulletType == BulletType.Boomerang)
            {
                transform.Rotate(0, 0, -rotateSpeed);
            }
        }
    }

    public void SetTargetPos(Vector3 newPos)
    {
        //set a target for bullet
        targetPos = newPos;
        if (bulletType == BulletType.Strait)
        {
            Vector3 rotate = new Vector3(0, 0, -90);
            rotate.y = attacker.transform.eulerAngles.y + 90;
            transform.eulerAngles = rotate;
        }
    }

    private void BoomerangBullet()
    {
        //first throw
        transform.position = Vector3.MoveTowards(transform.position, targetPos, bulletSpeed * Time.deltaTime);
        //return
        if (Vector3.Distance(transform.position, targetPos) <= 0.1f)
        {
            isReturned = true;
            transform.position = Vector3.MoveTowards(transform.position, attacker.position, bulletSpeed * Time.deltaTime);
        }

        //disable boomerang
        if (Vector3.Distance(transform.position, attacker.position) <= 0.1f
            && isReturned == true)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_PLAYER) || other.CompareTag(Constant.TAG_BOT))
        {
            //disable bullet if it hit character(player/bot)
            gameObject.SetActive(false);
            victim = other.transform;
            Character currentVictim = victim.transform.GetComponent<Character>();
            //if bullet hit other character => vitim die and update score for attacker
            if (victim != attacker && !currentVictim.IsDeath())
            {
                SoundManager.PlaySound(SoundType.WeaponHit);
                int victimScore = victim.GetComponent<Character>().currentPoint;
                attacker.GetComponent<Character>().AddScore(victimScore);
                victim.GetComponent<Character>().OnDie();
                //Update Player's killer name
                if (victim.gameObject.tag == Constant.TAG_PLAYER)
                {
                    GameManager.Instance.UpdateKillerName(attacker.GetComponent<Character>().characterName);
                    GameManager.Instance.GetReviveUI();
                }
            }
        }
        else if (!other.CompareTag(Constant.TAG_BULLET)
                && !other.CompareTag(Constant.TAG_TRANSPARENT))
        {
            gameObject.SetActive(false);
        }
    }
}
