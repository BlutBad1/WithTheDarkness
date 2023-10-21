using LightNS;
using UnityEngine;

namespace InteractableNS.Pickups
{
    public class OilPicking : Interactable
    {
        [SerializeField]
        private float disapperingSpeed;
        [SerializeField, Range(0, 100)]
        private int percentMinTime;
        [SerializeField, Range(0, 100)]
        private int percentMaxTime;
        protected override void Interact()
        {
            LightGlowTimer lightGlowTimer = UtilitiesNS.Utilities.GetComponentFromGameObject<LightGlowTimer>(LastWhoInteracted.gameObject);
            if (lightGlowTimer)
            {
                int addingTime;
                if (percentMinTime > percentMaxTime)
                    addingTime = (int)(Random.Range(0, percentMaxTime) * lightGlowTimer.MaxTimeLeft / 100);
                else
                    addingTime = (int)(Random.Range(percentMinTime, percentMaxTime) * lightGlowTimer.MaxTimeLeft / 100);
                lightGlowTimer.AddTime(addingTime);

            }
        }
    }
}