using ScenesManagementNS;
using UnityEngine;

namespace UiNS
{
    public class ButtonLoadScene : MonoBehaviour
    {
        [SerializeField]
        public GameObject gameObject;
        public void LoadSceneAndMoveGameObject(string scene)
        {
            Loader.Load(scene, gameObject, true);
        }
        public void LoadScene(string scene)
        {
            Loader.Load(scene);
        }
    }
}
