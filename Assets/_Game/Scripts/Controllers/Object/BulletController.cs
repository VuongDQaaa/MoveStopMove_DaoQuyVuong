using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Transform attacker;
    private Transform victim;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float rotateSpeed;
    private Vector3 targetPos = Vector3.zero;

    void Update()
    {
        if (targetPos != Vector3.zero)
        {
            //Bullet movement
            transform.position = Vector3.MoveTowards(transform.position, targetPos, bulletSpeed * Time.deltaTime);
            transform.Rotate(0, 0, -rotateSpeed);

            //Disable bullet if out of attack range
            if(Vector3.Distance(transform.position, targetPos) <= 0.1f)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void SetTargetPos(Vector3 newPos)
    {
        //set a target for bullet
        targetPos = newPos;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Constant.TAG_PLAYER) || other.CompareTag(Constant.TAG_BOT))
        {
            //disable bullet if it hit character(player/bot)
            gameObject.SetActive(false);
            victim = other.transform;
            Character currentVictim = victim.transform.GetComponent<Character>();
            //if bullet hit other character => vitim die and update score for attacker
            if(victim != attacker && !currentVictim.IsDeath())
            {
                int victimScore = victim.GetComponent<Character>().currentPoint;
                attacker.GetComponent<Character>().AddScore(victimScore);
                victim.GetComponent<Character>().OnDie();
                //Update Player's killer name
                if(victim.gameObject.tag == Constant.TAG_PLAYER)
                {
                    GameManager.Instance.UpdateKillerName(attacker.GetComponent<Character>().characterName);
                    GameManager.Instance.GetReviveUI();
                }
            }
        }
        else if(!other.CompareTag(Constant.TAG_BULLET) 
                && !other.CompareTag(Constant.TAG_TRANSPARENT))
        {
            gameObject.SetActive(false);
        }
    }
}
