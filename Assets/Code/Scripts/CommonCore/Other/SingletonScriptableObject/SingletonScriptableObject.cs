using System;
using UnityEngine;

namespace ScriptableObjectNS.Singleton
{
    public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
    {
        private static T instance;
        private static bool isInitializing = false;
        public static T Instance
        {
            get
            {
                if (instance == null && !isInitializing)
                {
                    isInitializing = true;
                    LoadInstance();
                }
                return instance;
            }
        }
        private static void LoadInstance()
        {
            try
            {
                T[] assets = Resources.LoadAll<T>("");
                if (assets == null || assets.Length < 1)
                {
                    Debug.LogWarning("Could not find any singleton scriptable object instances in the resources!");
                    instance = null;
                }
                else if (assets.Length > 1)
                {
                    Debug.LogWarning("Multiple instances of the singleton scriptable object found in the resources.");
                    instance = assets[0];
                }
                else
                    instance = assets[0];
            }
            catch (Exception e)// It throws an error anyway ;(
            {
            }
            finally
            {
                isInitializing = false;
            }
        }
    }
}
