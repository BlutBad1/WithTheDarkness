using LightNS;
using UnityEngine;

namespace EnvironmentEffects.InteractablePickup
{
    public class OilHighlight : PickupsHighlightEffect
    {
        [Range(0,100), Tooltip("Determining the percentage of low oil level.")]
        public float LevelOfLowOil = 15;
        public float MinDistanceOnLowOil = 20f;
        protected new void FixedUpdate()
        {
            if (LightGlowTimer.CurrentTimeLeft <= LevelOfLowOil)
                startDistanceOfEffect = MinDistanceOnLowOil;
            else
                startDistanceOfEffect = MinDistance;
            base.FixedUpdate();
        }
    }
}