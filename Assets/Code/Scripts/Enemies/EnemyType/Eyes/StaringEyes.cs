using PlayerScriptsNS;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UtilitiesNS.RendererNS;

namespace EnemyNS.Type.Eyes
{
    public class StaringEyes : MonoBehaviour
    {
        [SerializeField, FormerlySerializedAs("EyeRenderers")]
        private Renderer[] eyeRenderers;
        [SerializeField]
        private float waitTimeBeforeDisappear = 5f;

        private PlayerCreature playerCreature;
        private Coroutine currentCoroutine = null;
        private Camera mainCamera;

        private void Start()
        {
            if (!playerCreature)
                playerCreature = FindAnyObjectByType<PlayerCreature>();
            mainCamera = Camera.main;
        }
        private void Update()
        {
            if (CheckRenderVisibility.IsSomeRendererVisibleWithinCameraBounds(eyeRenderers, mainCamera) && currentCoroutine == null)
                currentCoroutine = StartCoroutine(PlayerSawEyes());
            transform.LookAt(playerCreature.gameObject.transform);
        }
        private IEnumerator PlayerSawEyes()
        {
            yield return new WaitForSeconds(waitTimeBeforeDisappear);
            gameObject.SetActive(false);
        }
    }
}
