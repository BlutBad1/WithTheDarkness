using ScenesManagementNS;
using UnityEngine;

namespace UiNS
{
    public class ButtonLoadScene : MonoBehaviour
    {
        [SerializeField]
        public GameObject gameObject;
        public void LoadTest(string scene)
        {
            Loader.LoadWithGameplay(scene);
        }
        public void LoadScene(string scene)
        {
            Loader.Load(scene);
        }
    }
}
