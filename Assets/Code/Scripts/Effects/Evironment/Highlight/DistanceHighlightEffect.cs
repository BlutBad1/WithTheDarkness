using PlayerScriptsNS;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnvironmentEffects.MatEffect.Highlight
{
    public class DistanceHighlightEffect : HighlightEffect
    {

        [SerializeField, FormerlySerializedAs("MinDistance"), Tooltip("Distance where effect starts to work")]
        private float minDistance = 10f;
        [SerializeField, FormerlySerializedAs("MaxIntensity")]
        private float maxIntensity = 1f;
        [SerializeField, FormerlySerializedAs("MinIntensity")]
        private float minIntensity = 0f;
        [SerializeField, FormerlySerializedAs("IntensityUpdateDelay")]
        private float intensityUpdateDelay = 0.1f;

        private float startDistanceOfEffect;
        private bool isEnable = true;
        private float currentIntensity;
        private GameObject player;
        private Coroutine currentCoroutine;

        protected float MinDistance { get => minDistance; }
        protected float StartDistanceOfEffect { get => startDistanceOfEffect; set => startDistanceOfEffect = value; }

        protected override void Start()
        {
            startIntensity = minIntensity;
            startDistanceOfEffect = minDistance;
            player = GameObject.FindAnyObjectByType<PlayerCreature>().gameObject;
            base.Start();
        }
        protected void FixedUpdate()
        {
            if (isEnable && player && currentCoroutine == null && Vector3.Distance(player.gameObject.transform.position, transform.position) <= startDistanceOfEffect)
                currentCoroutine = StartCoroutine(IntensityUpdate());
        }
        private void OnEnable()
        {
            isEnable = true;
        }
        private void OnDisable()
        {
            StopHighlighting();
        }
        public override void StopHighlighting()
        {
            base.StopHighlighting();
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
            isEnable = false;
        }
        private IEnumerator IntensityUpdate()
        {
            float previuslyIntensity = float.MinValue;
            while (Vector3.Distance(player.gameObject.transform.position, transform.position) <= startDistanceOfEffect)
            {
                currentIntensity = Mathf.Lerp(maxIntensity, minIntensity, Vector3.Distance(player.gameObject.transform.position, transform.position) / startDistanceOfEffect);
                //Prevents to update same intensity
                if (previuslyIntensity != currentIntensity)
                    SetIntensity(currentIntensity);
                previuslyIntensity = currentIntensity;
                yield return new WaitForSeconds(intensityUpdateDelay);
            }
            SetIntensity(minIntensity);
            currentCoroutine = null;
        }
    }
}
