using EntityNS.Base;
using SerializableTypes;
using System.Collections;
using UnityEngine;

namespace EntityNS.Attack
{
    public abstract class EntitySight : CreatureInSightChecker
    {
        [SerializeField]
        protected EntityBehaviour entityBehaviour;
        [SerializeField, Tooltip("What layers an enemy can see.")]
        protected LayerMask layersForProcessing;

        protected SerializableDictionary<GameObject, Coroutine> seenObjectsInSight = new SerializableDictionary<GameObject, Coroutine>();

        protected virtual void OnEnable()
        {
            OnGainSight += entityBehaviour.HandleGainCreatureInSight;
            OnLoseSight += entityBehaviour.HandleLoseCreatureFromSight;
        }
        protected virtual void OnDisable()
        {
            OnGainSight -= entityBehaviour.HandleGainCreatureInSight;
            OnLoseSight -= entityBehaviour.HandleLoseCreatureFromSight;
        }
        protected virtual void HandleGameObject(GameObject seenGameObject)
        {
            if (CheckIfNonEqualOrParent(seenGameObject, gameObject) && (layersForProcessing | (1 << seenGameObject.layer)) == layersForProcessing)
            {
                if (!seenObjectsInSight.ContainsKey(seenGameObject) || seenObjectsInSight[seenGameObject] == null)
                {
                    InvokeOnGainSight(seenGameObject);
                    seenObjectsInSight[seenGameObject] = StartCoroutine(CheckGameObjectWhileInSight(seenGameObject));
                }
            }
        }
        protected virtual bool CheckIfNonEqualOrParent(GameObject gameObjectThatHasParent, GameObject gameObject)
        {
            if (gameObjectThatHasParent == gameObject)
                return false;
            Transform currentParent = gameObjectThatHasParent.transform.parent;
            while (currentParent != null)
            {
                if (currentParent.gameObject == gameObject)
                    return false;
                currentParent = currentParent.transform.parent;
            }
            return true;
        }
        protected virtual IEnumerator CheckGameObjectWhileInSight(GameObject gameObject)
        {
            WaitForSeconds Wait = new WaitForSeconds(0.1f);
            while (CheckGameObjectInSight(gameObject))
                yield return Wait;
            InvokeOnLoseSight(gameObject);
            seenObjectsInSight.Remove(gameObject);
        }
        protected abstract bool CheckGameObjectInSight(GameObject gameObject);
    }
}