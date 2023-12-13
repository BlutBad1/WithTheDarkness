using UnityEngine;
namespace GameObjectsControllingNS
{
    public class ObjectSpawnChance<T, SupplyType> : ObjectSpawnChanceBase
        where T : MonoBehaviour
        where SupplyType : ObjectsSupply, new()
    {
        protected static SupplyType objectsSupplyInstance;

        public override void SetLowestSpawnChance() =>
            objectsSupplyInstance.SetLowestChance();
        public override void SetHightestSpawnChance() =>
          objectsSupplyInstance.SetHighestChance();
        protected override void AssignSupplyType()
        {
            if (objectsSupplyInstance == null)
                objectsSupplyInstance = new SupplyType();
        }
        protected override void Start()
        {
            AssignSupplyType();
            if (IsConnectedToSupply)
            {
                if (!objectsSupplyInstance.CalculateObjectChances(Chance))
                    Destroy(gameObject);
            }
            else if (Chance <= Random.Range(0, 100))
                Destroy(gameObject);
        }
    }
}