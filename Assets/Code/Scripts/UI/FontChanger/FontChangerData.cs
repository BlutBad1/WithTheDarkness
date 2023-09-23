using System;
using TMPro;
using UnityEngine;

namespace UINS.Font
{
    [CreateAssetMenu(fileName = "Font Data", menuName = "ScriptableObject/UI/FontData")]
    public class FontChangerData : ScriptableObject
    {
        public TMP_FontAsset FontAsset;
        public event Action OnFontChangeEvent;
        private void OnValidate()
        {
            try
            {
                OnFontChangeEvent?.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}
