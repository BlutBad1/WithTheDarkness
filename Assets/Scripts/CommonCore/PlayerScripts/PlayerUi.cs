using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace PlayerScriptsNS
{
public class PlayerUi : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;
 
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;   
    }
}
}