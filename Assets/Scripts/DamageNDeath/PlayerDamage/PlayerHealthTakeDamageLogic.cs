using HudNS;
using MyConstants;
using ScenesManagementNS;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerScriptsNS
{
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerHealthTakeDamageLogic : MonoBehaviour
    {
        [Header("Overlay")]
        public Image overlay;
        private Coroutine takeDamageCoroutine;
        private Coroutine deathCoroutine;
        BlackScreenDimming dimming;
        private void Start()
        {
            GetComponent<PlayerHealth>().OnTakeDamage += TakeDamage;
            GetComponent<PlayerHealth>().OnDeath += Death;
            dimming = GameObject.Find(HUDConstants.BLACK_SCREEN_DIMMING).GetComponent<BlackScreenDimming>();
        }
        private void OnDestroy()
        {
            GetComponent<PlayerHealth>().OnTakeDamage -= TakeDamage;
            GetComponent<PlayerHealth>().OnDeath -= Death;
        }
        public void TakeDamage(int damage, float force, Vector3 hit)
        {
            if (TryGetComponent(out CameraShake cameraShake))
                cameraShake.FooCameraShake(damage / 20);
            if (overlay != null && GetComponent<PlayerHealth>().Health < 100)
            {
                if (takeDamageCoroutine != null)
                    StopCoroutine(takeDamageCoroutine);
                takeDamageCoroutine = StartCoroutine(TakeDamageOverlayCoroutine());
            }
        }
        IEnumerator TakeDamageOverlayCoroutine()
        {
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, (100 - (float)GetComponent<PlayerHealth>().Health) / 100);
            while (GetComponent<PlayerHealth>().Health < 100)
            {
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, (100 - (float)GetComponent<PlayerHealth>().Health) / 100);
                yield return null;
            }
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, (100 - (float)GetComponent<PlayerHealth>().Health) / 100);
        }
        public void Death()
        {
            dimming.fadeSpeed = 2f;
            dimming.DimmingEnable();
            if (deathCoroutine != null)
                StopCoroutine(deathCoroutine);
            deathCoroutine = StartCoroutine(DeathCoroutine());
        }
        IEnumerator DeathCoroutine()
        {
            while (dimming.blackScreen.color.a < 1)
                yield return null;
            SceneManager sceneManager = GameObject.Find(MyConstants.SceneConstants.SCENE_MANAGER).GetComponent<SceneManager>();
            if (sceneManager)
                Loader.Load((int)sceneManager.SceneAfterLose);
            else
                Loader.Load(MyConstants.SceneConstants.MAIN_MENU);
        }
    }
}