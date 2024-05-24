using UnityEngine;
using UnityEngine.AI;

public class BotController : Character
{
    public NavMeshAgent agent;
    private IState currentState;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(new StopState());
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
        DetectTarget();
        Attack();
    }

    public void OnPatrol(Vector3 newPostion)
    {
        currentTarget = null;
        //move to a position
        ChangeAnim(AnimationState.run);
        isMoving = true;
        isAttack = false;
        agent.destination = newPostion;
    }

    public void OnStop()
    {
        //stop character
        agent.ResetPath();
        isMoving = false;
        if(isAttack == false && currentTarget == null)
        {
            ChangeAnim(AnimationState.idle);
        }
    }

    public void OnFindTarget()
    {
        //find the nearest character on the map
        float distance = Mathf.Infinity;
        GameObject targetCharacter = new GameObject(); 
        foreach (GameObject item in GameManager.Instance.currentCharacter)
        {
            float TargetDistance = Vector3.Distance(transform.position, item.transform.position);
            if(TargetDistance > 0.1f && TargetDistance < distance)
            {
                targetCharacter = item.gameObject;
                distance = TargetDistance;
            }
        }

        if(targetCharacter != null)
        {
            currentTarget = null;
            ChangeAnim(AnimationState.run);
            isAttack = false;
            isMoving = true;
            agent.destination = targetCharacter.transform.position;
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
