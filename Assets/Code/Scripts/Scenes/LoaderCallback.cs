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
                Loader.LoaderCallbackInstance1 = this;
                Loader.LoaderCallback();
            }
        }
    }
}