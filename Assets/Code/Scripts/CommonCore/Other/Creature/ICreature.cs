using UnityEngine;

namespace CreatureNS
{
    public interface ICreature
    {
        public string GetCreatureName();
        public GameObject GetCreatureGameObject();

        public static ICreature GetICreatureComponent(GameObject gameObject)
        {
            ICreature creature = gameObject.GetComponent<ICreature>() != null ? gameObject.GetComponent<ICreature>()
            : gameObject.GetComponentInParent<ICreature>() != null ? gameObject.GetComponentInParent<ICreature>()
            : gameObject.GetComponentInChildren<ICreature>();
            return creature;
        }
    }
}