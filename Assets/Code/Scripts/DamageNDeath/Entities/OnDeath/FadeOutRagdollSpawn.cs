using DecalNS;
using EnvironmentEffects.MatEffect.Dissolve;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace EntityNS.Death
{
    public class FadeOutRagdollSpawn : EntityDeadEvent
    {
        [SerializeField]
        private RagdollEnabler ragdollEnabler;
        [SerializeField]
        private Dissolve dissolve;
        [SerializeField, Tooltip("Time before the object will start to fade out.")]
        private float fadeOutDelay = 1f;
        [SerializeField, Tooltip("How much time the object need to full fade out.")]
        private float fadeOutTime = 0.05f;
        [SerializeField, Tooltip("At what point does the ragdoll dissolve turn off."), Range(-1, 1)]
        private float whenDisableRagdoll = 0f;
        [SerializeField]
        private GameObject mainGameObjectBody;
        [SerializeField]
        private GameObject fadeOutRagdollBody;

        private Coroutine fadeOutCoroutine;
        private bool isEnabled = false;

        protected override void OnDisable()
        {
            if ((fadeOutCoroutine != null || isEnabled) && gameObject.scene.IsValid())
                gameObject.SetActive(false);
        }
        protected override void OnDeadEvent()
        {
            base.OnDeadEvent();
            fadeOutCoroutine = StartCoroutine(FadeOutCoroutine());
            isEnabled = true;
            List<Decal> decals = UtilitiesNS.Utilities.FindAllComponentsInGameObject<Decal>(mainGameObjectBody);
            foreach (Decal decal in decals)
                MoveDecal(decal);
            mainGameObjectBody.SetActive(false);
            fadeOutRagdollBody.SetActive(true);
        }
        private void MoveDecal(Decal decal)
        {
            // Get the original path within the hierarchy
            string originalPath = GetGameObjectPath(decal.gameObject);
            string newPath = originalPath.Replace($"/{mainGameObjectBody.name}/", "~").Replace("/" + decal.gameObject.name, "");
            ParticleSystem[] enabledPaticles = UtilitiesNS.Utilities.FindAllComponentsInGameObject<ParticleSystem>(decal.gameObject).Where(x => x.isPlaying).ToArray();
            newPath = newPath.Remove(0, newPath.IndexOf("~") + 1);
            Transform newTransform = FindTransformByPath(newPath);
            decal.transform.parent = newTransform;
            foreach (var particle in enabledPaticles)
                particle.Play(true);
        }
        private string GetGameObjectPath(GameObject obj)
        {
            Transform current = obj.transform;
            string path = current.name;
            while (current.parent != null)
            {
                current = current.parent;
                path = current.name + "/" + path;
            }
            return path;
        }
        private Transform FindTransformByPath(string path)
        {
            Transform foundTransform = fadeOutRagdollBody.transform.Find(path.Substring(0, path.IndexOf("/") < 0 ? path.Length : path.IndexOf("/"))), nextTransform;
            nextTransform = foundTransform;
            // If the transform is not found, iteratively remove the last part of the path until found
            while (nextTransform != null && path.Length > 0)
            {
                path = path.Remove(0, path.IndexOf("/") + 1);
                foundTransform = nextTransform;
                nextTransform = foundTransform.Find(path.Substring(0, path.IndexOf("/") < 0 ? path.Length : path.IndexOf("/")));
            }
            return foundTransform;
        }
        private IEnumerator FadeOutCoroutine()
        {
            yield return new WaitForSeconds(fadeOutDelay);
            dissolve.InitializeMat();
            dissolve.StartDissolving(fadeOutTime);
            while (dissolve.CurrentDissolve < 1)
            {
                if (dissolve.CurrentDissolve >= whenDisableRagdoll && ragdollEnabler.IsRagdollEnabled)
                    ragdollEnabler.DisableAllRigidbodies();
                yield return null;
            }
            gameObject.SetActive(false);
        }
    }
}