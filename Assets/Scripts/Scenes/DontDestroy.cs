using UnityEngine;

namespace ScenesManagementNS
{
    public class DontDestroy : MonoBehaviour
    {
        static DontDestroy instance;
        private void Awake()
        {
            if (!instance)
                instance = this;
            else
                Destroy(this.gameObject);
            transform.SetParent(null);
            DontDestroyOnLoad(this.gameObject);
        }

    }
}