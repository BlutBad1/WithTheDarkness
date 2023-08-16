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
            int addingTime;
            if (percentMinTime > percentMaxTime)
                addingTime = (int)(Random.Range(0, percentMaxTime) * 100 / LightGlowTimer.StartedTimeLeft);
            else
                addingTime = (int)(Random.Range(percentMinTime, percentMaxTime) * 100 / LightGlowTimer.StartedTimeLeft);
            LightGlowTimer.AddTime(addingTime);
        }
    }
}