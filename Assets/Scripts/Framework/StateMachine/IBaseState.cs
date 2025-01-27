namespace Framework.StateMachine
{
    public interface IBaseState
    {
        public void OnUpdate();
        public void OnEnter();
        public void OnQuit();

    }
}