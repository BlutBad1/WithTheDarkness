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
        public static T GetComponentFromGameObject<T>(GameObject gameObject, bool includeSiblings = true) where T : class
        {
            T component = gameObject.GetComponent<T>();
            if (component != null) return component;
            Transform currentTransform = gameObject.transform;
            while (currentTransform.parent != null)
            {
                currentTransform = currentTransform.parent;
                component = currentTransform.GetComponent<T>();
                if (component != null)
                    return component;
            }
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(includeSiblings ? currentTransform : gameObject.transform);
            while (queue.Count > 0)
            {
                Transform current = queue.Dequeue();
                component = current.GetComponent<T>();
                if (component != null)
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
        public static void CopyAudioSourceSettings(AudioSource original, AudioSource destination)
        {
            destination.bypassEffects = original.bypassEffects;
            destination.bypassListenerEffects = original.bypassListenerEffects;
            destination.bypassReverbZones = original.bypassReverbZones;
            destination.clip = original.clip;
            destination.spread = original.spread;
            destination.dopplerLevel = original.dopplerLevel;
            destination.ignoreListenerPause = original.ignoreListenerPause;
            destination.ignoreListenerVolume = original.ignoreListenerVolume;
            destination.loop = original.loop;
            destination.maxDistance = original.maxDistance;
            destination.minDistance = original.minDistance;
            destination.pitch = original.pitch;
            destination.mute = original.mute;
            destination.outputAudioMixerGroup = original.outputAudioMixerGroup;
            destination.panStereo = original.panStereo;
            destination.playOnAwake = original.playOnAwake;
            destination.priority = original.priority;
            destination.reverbZoneMix = original.reverbZoneMix;
            destination.rolloffMode = original.rolloffMode;
            destination.spatialBlend = original.spatialBlend;
            destination.spatialize = original.spatialize;
            destination.spatializePostEffects = original.spatializePostEffects;
            destination.spread = original.spread;
            destination.volume = original.volume;
            destination.SetCustomCurve(AudioSourceCurveType.Spread, original.GetCustomCurve(AudioSourceCurveType.Spread));
            destination.SetCustomCurve(AudioSourceCurveType.CustomRolloff, original.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
            destination.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, original.GetCustomCurve(AudioSourceCurveType.ReverbZoneMix));
            destination.SetCustomCurve(AudioSourceCurveType.SpatialBlend, original.GetCustomCurve(AudioSourceCurveType.SpatialBlend));
        }
    }
}