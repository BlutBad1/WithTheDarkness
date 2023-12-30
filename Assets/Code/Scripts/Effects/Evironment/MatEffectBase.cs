using System.Collections.Generic;
using UnityEngine;


namespace EnvironmentEffects.MatEffect
{
    public class OriginalMatPropBlock
    {
        public Renderer Renderer;
        public Dictionary<int, MaterialPropertyBlock> MaterialPropertyBlock = new Dictionary<int, MaterialPropertyBlock>();
        public OriginalMatPropBlock(Renderer renderer, int index, MaterialPropertyBlock materialPropertyBlock)
        {
            Renderer = renderer;
            MaterialPropertyBlock.Add(index, materialPropertyBlock);
        }
    }
    public class MatEffectBase : MonoBehaviour
    {
        protected List<OriginalMatPropBlock> originalMatPropBlocks = new List<OriginalMatPropBlock>();
        public virtual void ResetAllRenderers()
        {
            foreach (var originalMPB in originalMatPropBlocks)
                ResetRenderer(originalMPB.Renderer);
        }
        public virtual void ResetRenderer(Renderer renderer)
        {
            OriginalMatPropBlock originalMatPropBlock = originalMatPropBlocks.Find(x => x.Renderer == renderer);
            if (originalMatPropBlock != null)
            {
                foreach (var key in originalMatPropBlock.MaterialPropertyBlock.Keys)
                    renderer.SetPropertyBlock(originalMatPropBlock.MaterialPropertyBlock[key], key);
            }
        }
        protected virtual void InitializeRenderer(Renderer renderer, MaterialPropertyBlock currentBlock, int i, Material referenceMat, Material[] rendererMaterials)
        {
            renderer.GetPropertyBlock(currentBlock, i);
            OriginalMatPropBlock originalMatPropBlock = originalMatPropBlocks.Find(x => x.Renderer == renderer);
            if (originalMatPropBlock == null)
                originalMatPropBlocks.Add(new OriginalMatPropBlock(renderer, i, currentBlock));
            else
            {
                if (!originalMatPropBlock.MaterialPropertyBlock.ContainsKey(i))
                    originalMatPropBlock.MaterialPropertyBlock.Add(i, currentBlock);
            }
            if (currentBlock.isEmpty)
            {
                Material originalMat = rendererMaterials[i];  // Use the local copy of materials array
                rendererMaterials[i] = referenceMat;  // Modify the material in the array
                renderer.GetPropertyBlock(currentBlock, i);
                if (originalMat.GetTexture("_MainTex"))
                    currentBlock.SetTexture("_MainTex", originalMat.GetTexture("_MainTex"));
                if (originalMat.HasProperty("_BumpMap") && originalMat.GetTexture("_BumpMap"))
                    currentBlock.SetTexture("_BumpMap", originalMat.GetTexture("_BumpMap"));
                if (originalMat.HasProperty("_Color"))
                    currentBlock.SetColor("_Color", originalMat.GetColor("_Color"));
                if (originalMat.HasProperty("_BumpScale"))
                    currentBlock.SetFloat("_BumpScale", originalMat.GetFloat("_BumpScale"));
                if (originalMat.HasProperty("_Metallic"))
                    currentBlock.SetFloat("_Metallic", originalMat.GetFloat("_Metallic"));
                if (originalMat.HasProperty("_Glossiness"))
                    currentBlock.SetFloat("_Glossiness", originalMat.GetFloat("_Glossiness"));
                if (originalMat.HasProperty("_SpecularHighlights"))
                    currentBlock.SetFloat("_SPECULARHIGHLIGHTS_OFF", 1 - originalMat.GetFloat("_SpecularHighlights"));
                if (originalMat.HasProperty("_GlossyReflections"))
                    currentBlock.SetFloat("_GLOSSYREFLECTIONS_OFF", 1 - originalMat.GetFloat("_GlossyReflections"));
            }
            else
            {
                MaterialPropertyBlock originalPropBlock = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(originalPropBlock, i);
                rendererMaterials[i] = referenceMat;  // Modify the material in the array
                renderer.GetPropertyBlock(currentBlock, i);
                if (originalPropBlock.GetTexture("_MainTex"))
                    currentBlock.SetTexture("_MainTex", originalPropBlock.GetTexture("_MainTex"));
                if (originalPropBlock.HasProperty("_BumpMap") && originalPropBlock.GetTexture("_BumpMap"))
                    currentBlock.SetTexture("_BumpMap", originalPropBlock.GetTexture("_BumpMap"));
                if (originalPropBlock.HasProperty("_Color"))
                    currentBlock.SetColor("_Color", originalPropBlock.GetColor("_Color"));
                if (originalPropBlock.HasProperty("_BumpScale"))
                    currentBlock.SetFloat("_BumpScale", originalPropBlock.GetFloat("_BumpScale"));
                if (originalPropBlock.HasProperty("_Metallic"))
                    currentBlock.SetFloat("_Metallic", originalPropBlock.GetFloat("_Metallic"));
                if (originalPropBlock.HasProperty("_Glossiness"))
                    currentBlock.SetFloat("_Glossiness", originalPropBlock.GetFloat("_Glossiness"));
                if (originalPropBlock.HasProperty("_SpecularHighlights"))
                    currentBlock.SetFloat("_SPECULARHIGHLIGHTS_OFF", 1 - originalPropBlock.GetFloat("_SpecularHighlights"));
                if (originalPropBlock.HasProperty("_GlossyReflections"))
                    currentBlock.SetFloat("_GLOSSYREFLECTIONS_OFF", 1 - originalPropBlock.GetFloat("_GlossyReflections"));
            }
        }
    }
}