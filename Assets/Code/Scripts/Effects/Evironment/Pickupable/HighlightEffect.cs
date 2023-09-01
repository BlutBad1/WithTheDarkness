using UnityEngine;

namespace EnvironmentEffects
{
    public class HighlightEffect : MonoBehaviour
    {
        [SerializeField] protected MeshRenderer[] meshRenderers;
        [SerializeField] protected Material referenceMat;
        protected MaterialPropertyBlock matPropBlock;
        protected float startIntensity = 0f;
        protected virtual void Start()
        {
            matPropBlock = new MaterialPropertyBlock();
            Texture texture;
            foreach (var renderer in meshRenderers)
            {
                texture = renderer.material.mainTexture;
                renderer.material = referenceMat;
                renderer.GetPropertyBlock(matPropBlock);
                matPropBlock.SetTexture("_MainTex", texture);
                matPropBlock.SetColor("_EmissionColor", referenceMat.GetColor("_EmissionColor") * startIntensity);
                renderer.SetPropertyBlock(matPropBlock);
            }
        }
        public void SetIntensity(float intensity)
        {
            if (matPropBlock != null)
            {
                foreach (var renderer in meshRenderers)
                {
                    //renderer.GetPropertyBlock(matPropBlock);
                    matPropBlock.SetColor("_EmissionColor", referenceMat.GetColor("_EmissionColor") * intensity);
                    renderer.SetPropertyBlock(matPropBlock);
                }
            }
        }
    }
}
