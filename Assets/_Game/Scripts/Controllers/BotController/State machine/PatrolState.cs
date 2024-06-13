public class PatrolState : IState
{
    public void OnEnter(BotController bot)
    {
        bot.OnPatrol();
    }

    public void OnExecute(BotController bot)
    {
        if (bot.agent.remainingDistance < 0.1f)
        {
            bot.ChangeState(new StopState());
        }

        if (bot.Death())
        {
            bot.agent.isStopped = true;
        }
    }

    public void OnExit(BotController bot)
    {

    }
}
