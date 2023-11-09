using LightNS;
using UnityEngine;

namespace EnvironmentEffects.MatEffect.Highlight
{
    public class OilHighlight : DistanceHighlightEffect
    {
        [Range(0, 100), Tooltip("Determining the percentage of low oil level.")]
        public float LevelOfLowOil = 15;
        public float MinDistanceOnLowOil = 20f;
        protected new void FixedUpdate()
        {
            LightGlowTimer lightGlowTimer = UtilitiesNS.Utilities.GetClosestComponent<LightGlowTimer>(transform.position);
            if (lightGlowTimer && lightGlowTimer.GetGlowingLeftTimeInPercantage() <= LevelOfLowOil)
                startDistanceOfEffect = MinDistanceOnLowOil;
            else if (lightGlowTimer)
                startDistanceOfEffect = MinDistance;
            base.FixedUpdate();
        }
    }
}