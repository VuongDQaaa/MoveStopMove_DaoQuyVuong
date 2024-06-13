public class FindTargetState : IState
{
    public void OnEnter(BotController bot)
    {
        bot.OnFindTarget();
    }

    public void OnExecute(BotController bot)
    {
        if(bot.agent.remainingDistance <= bot.attackRange - 0.5f)
        {
            bot.ChangeState(new StopState());
        }

        if(bot.Death())
        {
            bot.agent.isStopped = true;
        }    
    }

    public void OnExit(BotController bot)
    {
        
    }
}