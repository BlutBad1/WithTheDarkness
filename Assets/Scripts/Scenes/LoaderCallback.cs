using UnityEngine;

namespace ScenesManagementNS
{
    public class LoaderCallback : MonoBehaviour
    {
        private bool isFirstUpdate = true;
        private void Awake()
        {
            if (isFirstUpdate)
            {
                isFirstUpdate = false;
                if (!Loader.LoaderCallback())
                    StartCoroutine(Loader.LoadAndMoveGameObject());
            }
        }
    }
}