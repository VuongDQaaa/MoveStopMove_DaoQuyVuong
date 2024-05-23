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
        ChangeState(new IdleState());
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
        //move to a position
        ChangeAnim(AnimationState.run);
        isMoving = true;
        agent.destination = newPostion;
    }

    public void OnIdle()
    {
        //stand still and play idel anim
        agent.ResetPath();
        isMoving = false;
        ChangeAnim(AnimationState.idle);
    }

    public void OnFindTarget()
    {
        //find the nearest character on the map
        float distance = Mathf.Infinity;
        GameObject targetCharacter = new GameObject(); 
        foreach (Character item in GameManager.Instance.currentCharacter)
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
