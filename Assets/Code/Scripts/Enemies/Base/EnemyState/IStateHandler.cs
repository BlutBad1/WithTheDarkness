namespace EnemyNS.Base
{
    public interface IStateHandler
    {
        public void SetDefaultState(EnemyState newState);
        public void SetState(EnemyState newState);
        public EnemyState GetState();
    }
}