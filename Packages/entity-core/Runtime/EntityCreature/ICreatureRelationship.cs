using CreatureNS;

namespace EntityNS.Base
{
    public enum CreatureRelation
    {
        Friend, Enemy
    }
    public interface ICreatureRelationship : ICreature
    {
        public CreatureRelation DefineRelation(ICreature creature);
    }
}