using PlayerScriptsNS;
using UnityEditor;
using UnityEngine;
namespace EffectsNS.PlayerEffects
{
    public class ShakingTranformByPlayer : MonoBehaviour
    {
        [SerializeField]
        private PlayerMotor characterController;
        public AnimationCurve AnimationCurve;
        [SerializeField] private float shakeSpeed = 5f;
        [SerializeField] private float toDefaultPositionSpeed = 2f;
        [SerializeField] private float shakeMagnitude = 0.01f;
        [SerializeField] private float laterelEffect = 0.01f;
        [SerializeField, Tooltip("How much walkBobAmount would scale from player speed")] private float shakingExponent = 0.5f;
        [HideInInspector] public bool IsSpeedFactorLimited = true;
        [HideInInspector, Min(0)] public float MinLimit = 0f;
        [HideInInspector, Min(0)] public float MaxLimit = 1f;
        protected Transform whichTransformIsModifying;
        protected Vector3 defaultPosition;
        private Vector3 targetPosition;
        private float timer = 0;
        protected virtual void Awake()
        {
            if (!whichTransformIsModifying)
                whichTransformIsModifying = transform;
            defaultPosition = whichTransformIsModifying.localPosition;
        }
        protected virtual void Update()
        {
            Shaking();
        }
        public virtual void Shaking()
        {
            if (!characterController.IsGrounded)
                targetPosition = defaultPosition;
            else
            {
                float speedFactor = (characterController.GetCharacterVelocity()).magnitude / characterController.DefaultSpeed;
                if (IsSpeedFactorLimited)
                    speedFactor = Mathf.Clamp(speedFactor, MinLimit, MaxLimit);
                Vector3 shakeDirection = characterController.transform.InverseTransformDirection(characterController.GetCharacterVelocity()).normalized;
                if (speedFactor > 0.01f)
                {
                    timer += Time.deltaTime * shakeSpeed * Mathf.Pow(speedFactor, shakingExponent);
                    Vector3 desiredPosition = new Vector3(
                        defaultPosition.x + shakeDirection.x /** Mathf.Sin(timer) * shakeMagnitude*/ * laterelEffect,
                        defaultPosition.y + AnimationCurve.Evaluate(timer) /*Mathf.Sin(timer)*/ * shakeMagnitude * Mathf.Pow(speedFactor, shakingExponent),
                        defaultPosition.z + shakeDirection.z /** Mathf.Sin(timer) * shakeMagnitude*/ * laterelEffect
                    );
                    // Interpolate towards the desired position to smooth out the movement
                    targetPosition = Vector3.Lerp(targetPosition, desiredPosition, Time.deltaTime * shakeSpeed);
                }
                else
                    targetPosition = defaultPosition;
            }
            // Apply the target position
            whichTransformIsModifying.localPosition = Vector3.Lerp(whichTransformIsModifying.localPosition, targetPosition, targetPosition == defaultPosition ?
                  Time.deltaTime * toDefaultPositionSpeed
                : Time.deltaTime * shakeSpeed);
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(ShakingTranformByPlayer), true)]
    public class ShakingTranformByPlayer_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // for other non-HideInInspector fields
            ShakingTranformByPlayer script = (ShakingTranformByPlayer)target;
            // draw checkbox for the bool
            var property = serializedObject.FindProperty("IsSpeedFactorLimited");
            EditorGUILayout.PropertyField(property, new GUIContent("IsSpeedFactorLimited"), true);
            if (script.IsSpeedFactorLimited) // if bool is true, show other fields
            {
                property = serializedObject.FindProperty("MinLimit");
                EditorGUILayout.PropertyField(property, new GUIContent("MinLimit"), true);
                property = serializedObject.FindProperty("MaxLimit");
                EditorGUILayout.PropertyField(property, new GUIContent("MaxLimit"), true);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
