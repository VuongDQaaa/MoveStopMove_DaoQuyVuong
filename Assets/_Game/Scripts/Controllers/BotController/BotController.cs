using UnityEngine;
using UnityEngine.AI;

public class BotController : Character
{
    public NavMeshAgent agent;
    private IState currentState;
    private float maxMovingRange;
    private Transform targetCharacter;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(new StopState());
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeath == false)
        {
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }
            DetectTarget();
            Attack();
        }
    }

    public void OnPatrol()
    {
        //update information before moving
        isMoving = true;
        isAttack = false;

        //get a random postion on map
        maxMovingRange = Random.Range(15f, 20f);
        Vector3 randomDirection = Random.insideUnitCircle * maxMovingRange;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, maxMovingRange, 1);
        Vector3 finalPostion = hit.position;

        //move character
        ChangeAnim(AnimationState.run);
        agent.destination = finalPostion;
    }

    public void OnStop()
    {
        //stop character
        agent.ResetPath();
        isMoving = false;
        if (isAttack == false && currentTarget == null)
        {
            ChangeAnim(AnimationState.idle);
        }
    }

    public void OnFindTarget()
    {
        //find the nearest character on the map
        float distance = Mathf.Infinity;
        foreach (GameObject item in GameManager.Instance.currentCharacter)
        {
            float TargetDistance = Vector3.Distance(transform.position, item.transform.position);
            if (TargetDistance > 0.1f && TargetDistance < distance)
            {
                distance = TargetDistance;
                targetCharacter = item.transform;
            }
        }

        if (targetCharacter != null)
        {
            ChangeAnim(AnimationState.run);
            isAttack = false;
            isMoving = true;
            agent.destination = targetCharacter.position;
        }
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
}
