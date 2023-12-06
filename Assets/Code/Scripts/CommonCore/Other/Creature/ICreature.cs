using UnityEngine;

namespace CreatureNS
{
    public interface ICreature
    {
        public static ICreature GetICreatureComponent(GameObject gameObject)
        {
            ICreature creature = gameObject.GetComponent<ICreature>() != null ? gameObject.GetComponent<ICreature>()
            : gameObject.GetComponentInParent<ICreature>() != null ? gameObject.GetComponentInParent<ICreature>()
            : gameObject.GetComponentInChildren<ICreature>();
            return creature;
        }
        public string GetCreatureName();
        public GameObject GetCreatureGameObject();
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation);
        public void BlockMovement();
        public void UnBlockMovement();
    }
}