using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UtilitiesNS
{
    public class Utilities
    {
        public static int GetIndexOfElementInEnum<T>(T en) where T : IComparable, IFormattable, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("en must be enum type");
            var thisEnum = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            int index = 0;
            for (int i = 0; i < thisEnum.Count; i++)
            {
                if (en.Equals(thisEnum[i]))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        public static T GetComponentFromGameObject<T>(GameObject gameObject, bool includeSiblings = false) where T : class
        {
            T component = gameObject.GetComponent<T>();
            if (component != null && !component.Equals(null)) return component; // I don't know why, but sometimes component.Equals(null) is true and at the same time component != null is also true 
            Transform currentTransform = gameObject.transform;
            while (currentTransform.parent != null)
            {
                currentTransform = currentTransform.parent;
                component = currentTransform.GetComponent<T>();
                if (component != null && !component.Equals(null))
                    return component;
            }
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(includeSiblings ? currentTransform : gameObject.transform);
            while (queue.Count > 0)
            {
                Transform current = queue.Dequeue();
                component = current.GetComponent<T>();
                if (component != null && !component.Equals(null))
                    return component;
                foreach (Transform child in current)
                    queue.Enqueue(child);
            }
            return null;
        }
        public static T GetClosestComponent<T>(Vector3 referencePoint) where T : Component
        {
            T[] components = UnityEngine.Object.FindObjectsOfType<T>();
            T closestComponent = null;
            float closestDistance = float.MaxValue;
            foreach (T component in components)
            {
                float currentDistance = Vector3.Distance(referencePoint, component.transform.position);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestComponent = component;
                }
            }
            return closestComponent;
        }
        public static T GetClosestComponentInGameObject<T>(Vector3 referencePoint, GameObject gameObject) where T : Component
        {
            T[] components = FindAllComponentsInGameObject<T>(gameObject, false).ToArray();
            T closestComponent = null;
            float closestDistance = float.MaxValue;
            foreach (T component in components)
            {
                float currentDistance = Vector3.Distance(referencePoint, component.transform.position);
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestComponent = component;
                }
            }
            return closestComponent;
        }
        public static List<T> FindAllComponentsInGameObject<T>(GameObject gameObject, bool includeInactive = true) where T : Component
        {
            List<T> components = new List<T>();
            // Find components on the current GameObject
            T[] currentComponents = gameObject.GetComponents<T>();
            components.AddRange(includeInactive ? currentComponents : currentComponents.Where(x => x.gameObject.activeInHierarchy));
            // Find components on children recursively
            foreach (Transform child in gameObject.transform)
            {
                // Recursive call to find components on the child GameObject
                List<T> childComponents = FindAllComponentsInGameObject<T>(child.gameObject, includeInactive);
                components.AddRange(childComponents);
            }
            return components;
        }
    }
}