using SerializableTypes;
using UnityEngine;
namespace GameObjectsControllingNS
{
    public abstract class ObjectSpawnChanceBase : MonoBehaviour
    {
        [SerializeProperty("Chance"), SerializeField]
        private float chance;
        public virtual float Chance
        {
            get { return chance; }
            set { chance = value; }
        }
        public bool IsConnectedToSupply = false;
        public abstract void SetLowestSpawnChance();
        public abstract void SetHightestSpawnChance();
        protected abstract void AssignSupplyType();
        protected abstract void Start();
    }
}