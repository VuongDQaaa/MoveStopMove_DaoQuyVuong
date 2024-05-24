using UnityEngine;

public class PatrolState : IState
{
    Vector3 newPos = new Vector3();
    public void OnEnter(BotController bot)
    {
        newPos.x = Random.Range(-15, 15);
        newPos.y = bot.transform.position.y;
        newPos.z = Random.Range(-15, 15);

        bot.OnPatrol(newPos);
    }

    public void OnExecute(BotController bot)
    {
        if (bot.agent.remainingDistance < 0.1f)
        {
            bot.ChangeState(new StopState());
        }
    }

    public void OnExit(BotController bot)
    {

    }
}
