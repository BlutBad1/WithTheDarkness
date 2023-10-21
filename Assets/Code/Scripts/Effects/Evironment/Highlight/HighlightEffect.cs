using UnityEngine;

namespace EnvironmentEffects.MatEffect.Highlight
{
    public class HighlightEffect : MatEffectBase
    {
        [SerializeField] protected MeshRenderer[] meshRenderers;
        [SerializeField] protected Material referenceMat;
        protected MaterialPropertyBlock matPropBlock;
        protected float startIntensity = 0f;
        protected virtual void Start()
        {
            matPropBlock = new MaterialPropertyBlock();
            InitializeMat();
        }
        public virtual void InitializeMat()
        {
            MaterialPropertyBlock currentBlock = new MaterialPropertyBlock();
            foreach (var renderer in meshRenderers)
            {
                Material[] rendererMaterials = renderer.materials;  // Get the entire materials array
                for (int i = 0; i < rendererMaterials.Length; i++)
                {
                    InitializeRenderer(renderer, currentBlock, i, referenceMat, rendererMaterials);
                    currentBlock.SetColor("_EmissionColor", referenceMat.GetColor("_EmissionColor") * startIntensity);
                    renderer.SetPropertyBlock(currentBlock, i);
                }
                renderer.materials = rendererMaterials;  // Set the whole array back onto the renderer
            }
        }
        public void SetIntensity(float intensity)
        {
            if (matPropBlock != null)
            {
                foreach (var renderer in meshRenderers)
                {
                    for (int i = 0; i < renderer.materials.Length; i++)
                    {
                        renderer.GetPropertyBlock(matPropBlock, i);
                        matPropBlock.SetColor("_EmissionColor", referenceMat.GetColor("_EmissionColor") * intensity);
                        renderer.SetPropertyBlock(matPropBlock, i);
                    }
                }
            }
        }
        public virtual void StopHighlighting() =>
            SetIntensity(0);
    }
}
