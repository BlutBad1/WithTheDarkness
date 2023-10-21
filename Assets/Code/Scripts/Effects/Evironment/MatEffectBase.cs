using UnityEngine;


namespace EnvironmentEffects.MatEffect
{
    public class MatEffectBase : MonoBehaviour
    {
        protected virtual void InitializeRenderer(MeshRenderer renderer, MaterialPropertyBlock currentBlock, int i, Material referenceMat, Material[] rendererMaterials)
        {
            renderer.GetPropertyBlock(currentBlock, i);
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
            }
        }
    }
}