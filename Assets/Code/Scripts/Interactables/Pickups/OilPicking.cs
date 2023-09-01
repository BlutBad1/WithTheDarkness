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
                addingTime = (int)(Random.Range(0, percentMaxTime) * LightGlowTimer.StartedTimeLeft / 100);
            else
                addingTime = (int)(Random.Range(percentMinTime, percentMaxTime) * LightGlowTimer.StartedTimeLeft / 100);
            LightGlowTimer.AddTime(addingTime);
        }
    }
}