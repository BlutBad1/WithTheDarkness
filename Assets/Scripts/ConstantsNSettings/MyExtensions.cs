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
    }
}