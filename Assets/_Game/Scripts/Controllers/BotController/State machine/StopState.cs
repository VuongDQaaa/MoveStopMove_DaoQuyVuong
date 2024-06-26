using UnityEngine;

public class StopState : IState
{
    float timer;
    bool findTarget;
    public void OnEnter(BotController bot)
    {
        timer = Random.Range(2, 4);
        findTarget = Random.Range(0, 2) == 0;
    }

    public void OnExecute(BotController bot)
    {
        if (!bot.IsDeath())
        {
            if (timer > 0)
            {
                bot.OnStop();
                timer -= Time.deltaTime;
            }
            else if (timer <= 0 && findTarget)
            {
                bot.ChangeState(new FindTargetState());
            }
            else
            {
                bot.ChangeState(new PatrolState());
            }
        }
    }

    public void OnExit(BotController bot)
    {
        timer = 0;
    }
}
