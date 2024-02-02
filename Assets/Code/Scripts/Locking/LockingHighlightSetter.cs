using AYellowpaper;
using EnvironmentEffects.MatEffect.Highlight;
using ScriptableObjectNS.Locking;
using UnityEngine;

public class LockingHighlightSetter : MonoBehaviour
{
    [SerializeField]
    private ColorKeysData colorKeysData;
    [SerializeField, RequireInterface(typeof(ILockingInteractable))]
    private MonoBehaviour lockingInteractable;
    [SerializeField]
    private HighlightEffect highlightEffect;

    private bool isFirstFrame = true;

    private KeyData KeyData
    {
        get { return ((ILockingInteractable)lockingInteractable).GetKeyData(); }
    }

    private void Update()
    {
        if (isFirstFrame)
        {
            isFirstFrame = false;
            SetHighlightColor();
        }
    }
    private void SetHighlightColor()
    {
        highlightEffect.HightLightEmissionColor = colorKeysData.GetColorByKey(KeyData);
    }
}
