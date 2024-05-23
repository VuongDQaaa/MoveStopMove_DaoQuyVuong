using UnityEngine;

public class IdleState : IState
{
    float timer;
    public void OnEnter(BotController bot)
    {
        timer = Random.Range(1, 3);
    }

    public void OnExecute(BotController bot)
    {
        if(timer > 0)
        {
            bot.OnIdle();
            timer -= Time.deltaTime;
        }
        else
        {
            bot.ChangeState(new PatrolState());
        }
        
    }

    public void OnExit(BotController bot)
    {
        timer = 0;
    }
}
