using TMPro;
using UnityEngine;
namespace PlayerScriptsNS
{
    public class PlayerUi : MonoBehaviour, IPlayerUI
	{
        [SerializeField]
        private TextMeshProUGUI promptText;

        public void UpdateText(string promptMessage) =>
            promptText.text = promptMessage;
    }
}