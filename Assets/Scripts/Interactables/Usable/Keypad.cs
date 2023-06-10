using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractableNS.Usable
{
    public class Keypad : Interactable
    {
        [SerializeField]
        private GameObject door;
        private bool doorOpen;
   
        protected override void Interact()
        {
            doorOpen = !doorOpen;
            door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
        }
    }
}