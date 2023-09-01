using System.Collections;
using UnityEngine;

namespace EnvironmentEffects.InteractablePickup
{
    public class PickupsHighlightEffect : HighlightEffect
    {
        [Tooltip("Distance where effect starts to work")]
        public float MinDistance = 10f;
        public float MaxIntensity = 1f;
        public float MinIntensity = 0f;
        public float IntensityUpdateDelay = 0.1f;
        protected float startDistanceOfEffect;
        float currentIntensity;
        GameObject player;
        Coroutine currentCoroutine;
        protected override void Start()
        {
            startIntensity = MinIntensity;
            startDistanceOfEffect = MinDistance;
            player = GameObject.Find(MyConstants.CommonConstants.PLAYER);
            base.Start();
        }
        protected void FixedUpdate()
        {
            if (currentCoroutine == null && Vector3.Distance(player.gameObject.transform.position, transform.position) <= startDistanceOfEffect)
                currentCoroutine = StartCoroutine(IntensityUpdate());
        }
        private void OnDisable()
        {
            SetIntensity(MinIntensity);
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
        }
        IEnumerator IntensityUpdate()
        {
            float previuslyIntensity = float.MinValue;
            while (Vector3.Distance(player.gameObject.transform.position, transform.position) <= startDistanceOfEffect)
            {
                currentIntensity = Mathf.Lerp(MaxIntensity, MinIntensity, Vector3.Distance(player.gameObject.transform.position, transform.position) / startDistanceOfEffect);
                //Prevents to update same intensity
                if (previuslyIntensity != currentIntensity)
                    SetIntensity(currentIntensity);
                previuslyIntensity = currentIntensity;
                yield return new WaitForSeconds(IntensityUpdateDelay);
            }
            SetIntensity(MinIntensity);
            currentCoroutine = null;
        }
    }
}
