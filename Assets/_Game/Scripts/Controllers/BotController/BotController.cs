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
        OnInit();
        agent = GetComponent<NavMeshAgent>();
        ChangeState(new StopState());
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeath == false && GameManager.Instance.currentGameState == GameState.Playing)
        {
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }
            DetectTarget();
            Attack();
        }
    }

    private void OnInit()
    {
        //Update bot information before spawn
        int random = Random.Range(0, Constant.BOT_NAMES.Length - 1);
        characterName = Constant.BOT_NAMES[random];
        currentPoint = 0;

        //Update bot color
        Color botColor = GetRandomColor();
        bodyColor.material.color = botColor;
        pantColor.material.color = botColor;
    }

    private Color GetRandomColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        float a = 1f;
        return new Color(r, g, b, a);
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
        foreach (GameObject item in currentMap.currentCharacters)
        {
            float TargetDistance = Vector3.Distance(transform.position, item.transform.position);
            if (TargetDistance > 0.1f && TargetDistance < distance)
            {
                distance = TargetDistance;
                targetCharacter = item.transform;
            }
        }

        //Bot move to target
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
