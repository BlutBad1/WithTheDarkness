using System.Linq;
using UnityEngine;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static GameObject GetGameObject(this GameObject gameObject, string gameObjectName)
        {
            foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (!(go.hideFlags == HideFlags.NotEditable) && go.name == gameObjectName)
                    return go;
            }
            return null;
        }
        public static T GetComponentOrInherited<T>(this GameObject go) where T : Component
        {
            return go.GetComponents<Component>().OfType<T>().FirstOrDefault();
        }
    }
}