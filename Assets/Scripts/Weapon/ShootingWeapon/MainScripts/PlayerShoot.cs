using PlayerScriptsNS;
using System;
using UnityEngine;
namespace WeaponNS.ShootingWeaponNS
{
    public class PlayerShoot : MonoBehaviour
    {
        public static Action shootInput;
        public static Action reloadInput;
        private InputManager inputManager;

        //  [SerializeField] private KeyCode reloadKey;
        void Awake()
        {
            shootInput = null;
            reloadInput = null;
            inputManager = GetComponent<InputManager>();
        }
        public void Update()
        {
            if (inputManager.OnFoot.Firing.triggered)
            {
                shootInput?.Invoke();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (inputManager.OnFoot.Reloading.triggered)
                reloadInput?.Invoke();
           
        }
    }
}