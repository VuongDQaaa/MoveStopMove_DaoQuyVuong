public interface IState
{
    void OnEnter(BotController bot);
    void OnExecute(BotController bot);
    void OnExit(BotController bot);
}