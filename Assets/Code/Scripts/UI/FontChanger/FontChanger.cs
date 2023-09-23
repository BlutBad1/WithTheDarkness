using TMPro;
using UnityEngine;

namespace UINS.Font
{
    [RequireComponent(typeof(TextMeshProUGUI)),ExecuteInEditMode]
    public class FontChanger : MonoBehaviour
    {
        public FontChangerData FontChangerData;
        bool isSubsribed = false;
        private void Awake()
        {
            AttachToEvent();
        }
        private void OnDisable()
        {
            DettachToEvent();
        }
        private void OnDestroy()
        {
            DettachToEvent();
        }
        public void AttachToEvent()
        {
            if (FontChangerData && !isSubsribed)
            {
                FontChangerData.OnFontChangeEvent += ChangeFont;
                isSubsribed = true;
                ChangeFont();
            }
        }
        public void DettachToEvent()
        {
            if (FontChangerData && isSubsribed)
            {
                FontChangerData.OnFontChangeEvent -= ChangeFont;
                isSubsribed = false;
            }
        }
        public void ChangeFont()
        {
            if (FontChangerData && FontChangerData.FontAsset)
            {
                TextMeshProUGUI[] textsMeshPro = GetComponents<TextMeshProUGUI>();
                foreach (TextMeshProUGUI text in textsMeshPro)
                    text.font = FontChangerData.FontAsset;
            }
        }
    }
}
