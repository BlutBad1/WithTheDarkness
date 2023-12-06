using UnityEditor;
using UnityEngine;
namespace LocationManagementNS
{
    public enum TypeObjectToTeleport
    {
        Player, Creature, Gameobject
    }
    public class TeleportTrigger : MonoBehaviour
    {
        [EnumMask, HideInInspector]
        public TypeObjectToTeleport ObjectToTeleport;
        [HideInInspector]
        public LayerMask LayerObjectToTeleport;
        [HideInInspector]
        public Transform TeleportPointToHere;
        [HideInInspector]
        public Transform TeleportPoint; // position of the next spawn point of a next location 
        public bool IsConnectedToMapData = true;
        public bool DefineNextLocation = true;
        //thisLocIndex = -1, it means it's the first location. 
        //thisLocIndex = -2, it means it's the last location. 
        //connectedLocIndex = -2, it's connected to the last location. 
        protected int thisLocIndex = -1;
        protected int connectedLocIndex = -2;
        protected bool isLocIndexSet = false;
        private NextTeleportPointDefinition nextTeleportPointDefinition;
        protected virtual void Start()
        {
            if (IsConnectedToMapData)
                nextTeleportPointDefinition = gameObject.AddComponent<NextTeleportPointDefinition>();
        }
        public virtual int ThisLocIndex
        {
            get { return thisLocIndex; }
            set { thisLocIndex = isLocIndexSet ? thisLocIndex : value; isLocIndexSet = true; }
        }
        public virtual int ConnectedLocIndex
        {
            get { return connectedLocIndex; }
            set { connectedLocIndex = value; }
        }
        protected void DefineNextLoc()
        {
            if (DefineNextLocation && IsConnectedToMapData)
                nextTeleportPointDefinition.DefineNextLoc();
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(TeleportTrigger), true)]
    public class TeleportTrigger_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            TeleportTrigger script = (TeleportTrigger)target;
            // draw checkbox for the bool
            var property = serializedObject.FindProperty("ObjectToTeleport");
            EditorGUILayout.PropertyField(property, new GUIContent("ObjectToTeleport"), true);

            property = serializedObject.FindProperty("LayerObjectToTeleport");
            EditorGUILayout.PropertyField(property, new GUIContent("LayerObjectToTeleport"), true);

            property = serializedObject.FindProperty("TeleportPointToHere");
            EditorGUILayout.PropertyField(property, new GUIContent("TeleportPointToHere"), true);

            if (!script.IsConnectedToMapData)
            {
                property = serializedObject.FindProperty("TeleportPoint");
                EditorGUILayout.PropertyField(property, new GUIContent("TeleportPoint"), true);
            }
            serializedObject.ApplyModifiedProperties();
            DrawDefaultInspector(); // for other non-HideInInspector fields
        }
    }
#endif
}