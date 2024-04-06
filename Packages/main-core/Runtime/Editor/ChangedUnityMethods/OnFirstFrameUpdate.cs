using UnityEngine;

namespace UnityMethodsNS
{
    public class OnFirstFrameUpdate : MonoBehaviour
    {
        private bool isFirstFrame = false;

        private void Update()
        {
            if (!isFirstFrame)
            {
                isFirstFrame = true;
                OnFirstFrame();
            }
        }
        protected virtual void OnFirstFrame() { }
    }
}