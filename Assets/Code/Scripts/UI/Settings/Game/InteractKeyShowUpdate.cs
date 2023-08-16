using SettingsNS;
using TMPro;
using UnityEngine;

namespace UIControlling
{
    public class InteractKeyShowUpdate : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text interacteKeyText;
        string isTextUpdated;
        private void Start()
        {
            isTextUpdated = interacteKeyText.text;
        }
        private void OnDisable()
        {
            if (isTextUpdated != interacteKeyText.text)
            {
                GameSettings.OnInteracteRebind?.Invoke();
                isTextUpdated = interacteKeyText.text;
            }
        }
    }
}