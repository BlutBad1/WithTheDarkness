using UnityEngine;
using UnityEngine.UI;

namespace ForTestingNS
{
    public class CenterOfScreen : MonoBehaviour
    {
        public Image Crosshair;
        void Start()
        {
            float x = (Screen.width / 2) ;
            float y = (Screen.height / 2);
            Crosshair.transform.position = new Vector2(x, y);
        }
    }
}
