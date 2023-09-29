using EffectsNS.PlayerEffects;
using HudNS;
using InteractableNS;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace ScenesManagementNS
{
    public class ToNextScene : ProgressingInteractable
    {
        BlackScreenDimming bSD;
        public float BlackScreenFadeSpeed = 0.8f;
        public bool UseAsTrigger = false;
        [HideInInspector]
        public bool DefineByProgressManager = true;
        [HideInInspector]
        public string NextScene;
        protected override void Interact()
        {
            bSD = LastWhoInteracted.gameObject.transform.parent.GetComponentInChildren<BlackScreenDimming>();
            if (bSD)
            {
                bSD.FadeSpeed = BlackScreenFadeSpeed;
                bSD.DimmingEnable();
            }
            StartCoroutine(ToTheNextScene());
        }
        private void OnTriggerEnter(Collider other)
        {
            if (UseAsTrigger)
            {
                if (other.gameObject.TryGetComponent(out LastWhoInteracted))
                    StartBaseInteraction(LastWhoInteracted);
            }
        }
        IEnumerator ToTheNextScene()
        {
            while (bSD != null && bSD.BlackScreen.color.a < 0.95f)
                yield return null;
            if (DefineByProgressManager)
            {
                SceneDeterminant sceneManager = GameObject.Find(MyConstants.SceneConstants.PROGRESS_MANAGER).GetComponent<SceneDeterminant>();
                if (sceneManager)
                    Loader.Load(sceneManager.GetRandomScene(sceneManager.NextScenes, sceneManager.NextScenesSpawnChances));
                else
                    Loader.Load(MyConstants.SceneConstants.MAIN_MENU);
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
            script.DefineByProgressManager = EditorGUILayout.Toggle("Define By Progress Manager", script.DefineByProgressManager);
            if (!script.DefineByProgressManager) // if bool is false, show other fields
                script.NextScene = EditorGUILayout.TextField("Scene Name", script.NextScene);
        }
    }
#endif
}