using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnvironmentEffects.MatEffect.Dissolve
{
    public class Dissolve : MatEffectBase
    {
        [SerializeField]
        public List<MeshRenderer> meshRenderers;
        [SerializeField]
        public Material referenceMat;
        [Range(-1, 1f), Tooltip("-1 is full disabled dissolve, 1 is full enabled")]
        public float StartedDissolve = -1f;
        [SerializeField]
        public float updateRate = 0.01f;
        public bool ActiveOnStart = false;
        protected MaterialPropertyBlock matPropBlock;
        protected Coroutine currentCoroutine;
        protected float currentDissolve;
        protected float edgeThickness;
        protected bool isMatInitialized = false;
        public float CurrentDissolve
        { get { return currentDissolve; } }

        protected virtual void Start()
        {
            matPropBlock = new MaterialPropertyBlock();
            if (ActiveOnStart)
                InitializeMat();
        }
        public virtual void InitializeMat()
        {
            currentDissolve = StartedDissolve;
            MaterialPropertyBlock currentBlock = new MaterialPropertyBlock();
            foreach (var renderer in meshRenderers)
            {
                Material[] rendererMaterials = renderer.materials;  // Get the entire materials array
                for (int i = 0; i < rendererMaterials.Length; i++)
                {
                    InitializeRenderer(renderer, currentBlock, i, referenceMat, rendererMaterials);
                    currentBlock.SetFloat("_DissolveTime", StartedDissolve);
                    currentBlock.SetFloat("_EdgeThickness", 0);
                    renderer.SetPropertyBlock(currentBlock, i);
                }
                renderer.materials = rendererMaterials;  // Set the whole array back onto the renderer
            }
            isMatInitialized = true;
        }
        public virtual void SetDissolve(float dissolve)
        {
            currentDissolve = dissolve;
            foreach (var renderer in meshRenderers)
            {
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    renderer.GetPropertyBlock(matPropBlock, i);
                    matPropBlock.SetFloat("_EdgeThickness", referenceMat.GetFloat("_EdgeThickness"));
                    matPropBlock.SetFloat("_DissolveTime", dissolve);
                    renderer.SetPropertyBlock(matPropBlock, i);
                }
            }
        }
        public virtual void StartEmerging(float dissolveTime) =>
            DissolveTo(-1, dissolveTime);
        public virtual void StartDissolving(float dissolveTime) =>
            DissolveTo(1, dissolveTime);
        public virtual void DissolveTo(float dissolve, float dissolveTime)
        {
            if (!isMatInitialized)
                InitializeMat();
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(ChangeDissolveStatus(dissolve, dissolveTime));
        }
        protected IEnumerator ChangeDissolveStatus(float dissolve, float dissolveTime)
        {
            float time = 0;
            while (Mathf.Abs(dissolve - currentDissolve) >= 0.01)
            {
                currentDissolve = Mathf.Lerp(currentDissolve, dissolve, time);
                time += Time.deltaTime / dissolveTime;
                SetDissolve(currentDissolve);
                yield return new WaitForSeconds(updateRate);
            }
            currentDissolve = dissolve;
            foreach (var renderer in meshRenderers)
            {
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    renderer.GetPropertyBlock(matPropBlock, i);
                    matPropBlock.SetFloat("_EdgeThickness", 0);
                    renderer.SetPropertyBlock(matPropBlock, i);
                }
            }
            currentCoroutine = null;
        }
    }
}