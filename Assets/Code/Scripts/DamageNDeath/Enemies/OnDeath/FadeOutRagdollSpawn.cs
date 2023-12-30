using DecalNS;
using EnvironmentEffects.MatEffect.Dissolve;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace EnemyNS.Death
{
    public class FadeOutRagdollSpawn : EnemyDeadEvent
    {
        public RagdollEnabler RagdollEnabler;
        public Dissolve Dissolve;
        [Tooltip("Delay to fade out.")]
        public float FadeOutDelay = 1f;
        [Tooltip("Fade out time.")]
        public float FadeOutTime = 0.05f;
        [Tooltip("At what point does the ragdoll dissolve turn off."), Range(-1, 1)]
        public float WhenDisableRagdoll = 0f;
        protected Coroutine fadeOutCoroutine;
        protected bool isEnabled = false;
        public GameObject MainGameObjectBody;
        public GameObject FadeOutRagdollBody;
        private void OnDisable()
        {
            if ((fadeOutCoroutine != null || isEnabled) && gameObject.scene.IsValid())
                gameObject.SetActive(false);
        }
        public override void OnDead()
        {
            base.OnDead();
            fadeOutCoroutine = StartCoroutine(FadeOutCoroutine());
            isEnabled = true;
            List<Decal> decals = UtilitiesNS.Utilities.FindAllComponentsInGameObject<Decal>(MainGameObjectBody);
            foreach (Decal decal in decals)
                MoveDecal(decal);
            MainGameObjectBody.SetActive(false);
            FadeOutRagdollBody.SetActive(true);
        }
        private void MoveDecal(Decal decal)
        {
            // Get the original path within the hierarchy
            string originalPath = GetGameObjectPath(decal.gameObject);
            string newPath = originalPath.Replace($"/{MainGameObjectBody.name}/", "~").Replace("/" + decal.gameObject.name, "");
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
            Transform foundTransform = FadeOutRagdollBody.transform.Find(path.Substring(0, path.IndexOf("/") < 0 ? path.Length : path.IndexOf("/"))), nextTransform;
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
            yield return new WaitForSeconds(FadeOutDelay);
            Dissolve.InitializeMat();
            Dissolve.StartDissolving(FadeOutTime);
            while (Dissolve.CurrentDissolve < 1)
            {
                if (Dissolve.CurrentDissolve >= WhenDisableRagdoll && RagdollEnabler.IsRagdollEnabled)
                    RagdollEnabler.DisableAllRigidbodies();
                yield return null;
            }
            gameObject.SetActive(false);
        }
    }
}