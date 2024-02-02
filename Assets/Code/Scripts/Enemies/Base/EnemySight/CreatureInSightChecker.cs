using UnityEngine;

namespace EnemyNS.Attack
{
    public abstract class CreatureInSightChecker : MonoBehaviour
    {
        public delegate void GainSightEvent(GameObject gameObject);
        public delegate void LoseSightEvent(GameObject gameObject);
        public event GainSightEvent OnGainSight;
        public event LoseSightEvent OnLoseSight;

        protected void InvokeOnGainSight(GameObject gameObject) =>
            OnGainSight?.Invoke(gameObject);
        protected void InvokeOnLoseSight(GameObject gameObject) =>
            OnLoseSight?.Invoke(gameObject);
    }
}