namespace EntityNS.Base
{
    public interface IStateHandler
    {
        public EntityState CurrentState { get; set; }
        public EntityState DefaultState { get; set; }
    }
}