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
        public static T GetComponentFromGameObject<T>(GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component != null) return component;
            Transform currentTransform = gameObject.transform;
            while (currentTransform.parent != null)
            {
                currentTransform = currentTransform.parent;
                component = currentTransform.GetComponent<T>();
                if (component != null) return component;
            }
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(currentTransform);
            while (queue.Count > 0)
            {
                Transform current = queue.Dequeue();
                component = current.GetComponent<T>();
                if (component != null) return component;
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
    }
}