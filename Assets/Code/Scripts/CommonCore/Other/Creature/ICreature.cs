using UnityEngine;

namespace CreatureNS
{
    public interface ICreature
    {
        public string GetCreatureName();
        public GameObject GetCreatureGameObject();
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation);
        public void BlockMovement();
        public void UnblockMovement();
        public void SetSpeedCoef(float speedCoef);
    }
}