public class StopState : IState
{
    public void OnEnter(BotController bot)
    {
        bot.isMoving = false;
        bot.agent.ResetPath();
    }

    public void OnExecute(BotController bot)
    {
        if(bot.agent.remainingDistance < bot.attackRange - 0.5f)
        {
            bot.agent.ResetPath();
        }
    }

    public void OnExit(BotController bot)
    {
        
    }
}
