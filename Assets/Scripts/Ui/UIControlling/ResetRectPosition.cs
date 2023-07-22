using UnityEngine;

namespace UIControlling
{
    public class ResetRectPosition : MonoBehaviour
    {
        public RectTransform RectTransform;
        Vector2 defaultPosition;
        private void Awake()
        {
            if (!RectTransform)
                RectTransform = GetComponent<RectTransform>();
            defaultPosition = RectTransform.anchoredPosition;
        }
        public void ResetPosition() =>
            RectTransform.anchoredPosition = defaultPosition;
    }
}