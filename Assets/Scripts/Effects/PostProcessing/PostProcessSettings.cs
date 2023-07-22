using SettingsNS;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace PostProcessingNS
{
    [RequireComponent(typeof(PostProcessVolume))]
    public class PostProcessSettings : MonoBehaviour
    {
        public PostProcessProfile postProcessLDR;
        public PostProcessProfile postProcessHDR;
        PostProcessVolume postProcessVolume;
        void Start()
        {
            postProcessVolume = GetComponent<PostProcessVolume>();
            ChangePostProcessProfile();
            GraphicSettings.OnHDRChange += ChangePostProcessProfile;
            GraphicSettings.OnBrightnessChange += ChangeBrightnessPostProcess;
        }
        public void ChangePostProcessProfile()
        {
            if (GraphicSettings.HDROn)
                postProcessVolume.profile = postProcessHDR;
            else
                postProcessVolume.profile = postProcessLDR;
            ChangeBrightnessPostProcess();
        }
        public void ChangeBrightnessPostProcess()
        {
            if (postProcessVolume.profile.TryGetSettings(out AutoExposure exposure))
                exposure.keyValue.value = GraphicSettings.Brightness;
        }
    }
}