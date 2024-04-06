using UnityEngine;

namespace CreatureNS
{
    public interface ICreature
    {
        public float CurrentSpeedCoefficient { get; set; }

        public string GetCreatureType();
        public GameObject GetCreatureGameObject();
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation);
        public void BlockMovement();
        public void UnblockMovement();
    }
}