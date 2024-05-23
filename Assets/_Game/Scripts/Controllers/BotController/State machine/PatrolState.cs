using UnityEngine;

public class PatrolState : IState
{
    Vector3 newPos = new Vector3();
    bool findTarget;
    public void OnEnter(BotController bot)
    {
        newPos.x = Random.Range(0, 15);
        newPos.y = bot.transform.position.y;
        newPos.z = Random.Range(0, 15);
        findTarget = Random.Range(0, 2) == 0;
        bot.OnPatrol(newPos);
    }

    public void OnExecute(BotController bot)
    {
        if (bot.agent.remainingDistance < 0.1f && !findTarget)
        {
            bot.ChangeState(new IdleState());
        }
        else if (findTarget)
        {
            bot.ChangeState(new FindTargetState());
        }
    }

    public void OnExit(BotController bot)
    {
        newPos = new Vector3();
    }
}
