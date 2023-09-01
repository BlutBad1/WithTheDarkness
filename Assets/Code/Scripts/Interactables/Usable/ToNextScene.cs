using HudNS;
using InteractableNS;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace ScenesManagementNS
{
    public class ToNextScene : Interactable
    {
        BlackScreenDimming bSD;
        public float BlackScreenFadeSpeed = 0.8f;
        public bool IsTrigger = false;
        [HideInInspector]
        public bool DefineByProgressManager = true;
        [HideInInspector]
        public string NextScene;
        protected override void Interact()
        {
            bSD = WhoInteracted.gameObject.transform.parent.GetComponentInChildren<BlackScreenDimming>();
            if (bSD)
            {
                bSD.fadeSpeed = BlackScreenFadeSpeed;
                bSD.DimmingEnable();
            }
            StartCoroutine(ToTheNextScene());
        }
        private void OnTriggerEnter(Collider other)
        {
            if (IsTrigger)
            {
                if (other.gameObject.TryGetComponent(out WhoInteracted))
                    BaseInteract(WhoInteracted);
            }
        }
        IEnumerator ToTheNextScene()
        {
            while (bSD != null && bSD.blackScreen.color.a < 0.95f)
                yield return null;
            if (DefineByProgressManager)
            {
                SceneDeterminant sceneManager = GameObject.Find(MyConstants.SceneConstants.PROGRESS_MANAGER).GetComponent<SceneDeterminant>();
                if (sceneManager)
                    Loader.Load((int)sceneManager.NextScene);
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