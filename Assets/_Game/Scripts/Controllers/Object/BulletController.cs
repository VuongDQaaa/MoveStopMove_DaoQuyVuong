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
    [SerializeField] private Vector3 targetPos = Vector3.zero;
    [SerializeField] private Vector3 startPos = Vector3.zero;
    [SerializeField] private bool isReturned;

    private void OnEnable()
    {
        isReturned = false;
        startPos = attacker.position;
    }

    private void Update()
    {
        if (bulletType == BulletType.Boomerang)
        {
            BoomerangBullet();
        }
        else
        {
            NormalBulletControl();
        }

        RotateBullet();
    }

    private void RotateBullet()
    {
        //Bullet rotate
        if (bulletType == BulletType.Spin || bulletType == BulletType.Boomerang)
        {
            transform.Rotate(0, 0, -rotateSpeed);
        }
    }

    private void NormalBulletControl()
    {
        if (targetPos != Vector3.zero)
        {
            //Bullet movement
            transform.position = Vector3.MoveTowards(transform.position, targetPos, bulletSpeed * Time.deltaTime);
            //Disable bullet if out of attack range
            if (Vector3.Distance(transform.position, targetPos) <= 0.1f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void BoomerangBullet()
    {
        //First throw
        if (targetPos != Vector3.zero && !isReturned)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, bulletSpeed * Time.deltaTime);
        }

        //condition to return
        if (Vector3.Distance(transform.position, targetPos) <= 0.1f && gameObject.activeSelf)
        {
            Debug.Log("return");
            isReturned = true;
        }

        //return
        if(isReturned)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, bulletSpeed * Time.deltaTime);
        }

        //disable when return
        if(Vector3.Distance(transform.position, startPos) <= 0.1f && isReturned)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetTargetPos(Vector3 target)
    {
        //set a target for bullet
        targetPos = target;

        //set direction for strait bullet
        if (bulletType == BulletType.Strait)
        {
            Vector3 rotate = new Vector3(0, 0, -90);
            rotate.y = attacker.transform.eulerAngles.y + 90;
            transform.eulerAngles = rotate;
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
