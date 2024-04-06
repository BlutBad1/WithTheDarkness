using UnityEngine;
namespace GameObjectsControllingNS
{
    public abstract class ObjectSpawnChance<T, SupplyType> : ObjectSpawnChanceBase
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
            TrySpawnObject();
        }
        private void TrySpawnObject()
        {
            if (IsConnectedToSupply)
            {
                if (!objectsSupplyInstance.CalculateObjectChances(Chance))
                    gameObject.SetActive(false);
            }
            else if (Chance <= Random.Range(0, 100))
                gameObject.SetActive(false);
        }
    }
}
