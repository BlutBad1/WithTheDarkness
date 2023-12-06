using UnityEngine;
namespace DifferentUnityMethods
{
    public class MethodsBeforeQuit : MonoBehaviour
    {
        protected bool applicationQuit = false;
        public void OnApplicationQuit()
        {
            this.applicationQuit = true;
        }
        public void OnDisable()
        {
            if (!this.applicationQuit)
                this.OnDisableBeforeQuit();
        }
        public virtual void OnDisableBeforeQuit() { }
        public void OnDestroy()
        {
            if (!this.applicationQuit)
                this.OnDestroyBeforeQuit();
        }
        public virtual void OnDestroyBeforeQuit() { }
    }
}