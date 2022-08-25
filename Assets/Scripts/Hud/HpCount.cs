using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HpCount : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;
    void Start()
    {
        
    }

  
        public void UpdateText(string promptMessage)
        {
            promptText.text = promptMessage;
        }
   
}
