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
        public static T GetComponentOrInherited<T>(this GameObject go) where T : Component =>
             go.GetComponents<Component>().OfType<T>().FirstOrDefault();
        public static bool CheckIfLayerInLayerMask(this LayerMask layerMask, int layer) =>
            layerMask == (layerMask | (1 << layer));
    }
}