using EffectsNS.PlayerEffects;
using InteractableNS;
using SceneConstantsNS;
using SoundNS;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScenesManagementNS
{
    public class ToNextScene : ProgressingInteractable
    {
        [SerializeField, FormerlySerializedAs("MuteSound")]
        private MuteSound muteSound;
        [SerializeField, FormerlySerializedAs("MuteSound")]
        private float blackScreenFadeSpeed = 0.8f;
        [SerializeField, FormerlySerializedAs("MuteSound")]
        private float muteSoundTranTime = 10f;
        [SerializeField, FormerlySerializedAs("MuteSound")]
        private bool useAsTrigger = false;
        [SerializeField]
        private bool defineByProgressManager = true;

        private BlackScreenDimming bSD;

        public bool DefineByProgressManager { get => defineByProgressManager; }
        public string NextScene { get; set; }

        protected override void Interact()
        {
            bSD = LastWhoInteracted.gameObject.transform.parent.GetComponentInChildren<BlackScreenDimming>();
            if (bSD)
            {
                bSD.FadeSpeed = blackScreenFadeSpeed;
                bSD.DimmingEnable();
            }
            muteSound?.MuteVolume(muteSoundTranTime);
            StartCoroutine(ToTheNextScene());
        }
        private void OnTriggerEnter(Collider other)
        {
            if (useAsTrigger)
            {
                if (other.gameObject.TryGetComponent(out EntityInteract lastWhoInteracted))
                {
                    LastWhoInteracted = lastWhoInteracted;
                    Interact();
                }
            }
        }
        IEnumerator ToTheNextScene()
        {
            while (bSD != null && bSD.BlackScreen.color.a < 0.95f)
                yield return null;
            if (DefineByProgressManager)
            {
                SceneDeterminant sceneManager = GameObject.Find(SceneConstants.PROGRESS_MANAGER).GetComponent<SceneDeterminant>();
                if (sceneManager)
                    Loader.Load(sceneManager.GetRandomNextScene());
                else
                    Loader.Load(SceneConstants.MAIN_MENU);
            }
            else
                Loader.Load(NextScene);
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(ToNextScene))]
    public class ToNextScene_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // for other non-HideInInspector fields
            ToNextScene script = (ToNextScene)target;
            // draw checkbox for the bool
            if (!script.DefineByProgressManager) // if bool is false, show other fields
                script.NextScene = EditorGUILayout.TextField("Scene Name", script.NextScene);
        }
    }
#endif
}