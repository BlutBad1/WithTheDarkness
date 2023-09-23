using SerializableTypes;
using UnityEngine;
namespace GameObjectsControllingNS
{
    public class ObjectSpawnChance<T, SupplyType> : MonoBehaviour where SupplyType : ObjectsSupply, new()
    {
        [SerializeProperty("Chance"), SerializeField]
        private float chance;
        protected static SupplyType objectsSupplyInstance;
        public virtual float Chance
        {
            get { return chance; }
            set { chance = value; }
        }
        public bool IsConnectedToSupply = false;
        protected virtual void AssignSupplyType()
        {
            if (objectsSupplyInstance == null)
                objectsSupplyInstance = new SupplyType();
        }
        protected virtual void Awake()
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
