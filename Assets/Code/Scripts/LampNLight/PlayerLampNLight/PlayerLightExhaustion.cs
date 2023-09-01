using PlayerScriptsNS;
using UnityEngine;
namespace LightNS.Player
{
    public class PlayerLightExhaustion : MonoBehaviour
    {
        public float Damage = 20f;
        public float TimeBetweenAttacks = 5f;
        public PlayerHealth PlayerHealth;
        float timeSinceLastAttack = 0f;
        private void Update()
        {
            if (LightGlowTimer.CurrentTimeLeft <= 0)
            {
                timeSinceLastAttack += Time.deltaTime;
                if (timeSinceLastAttack > TimeBetweenAttacks)
                {
                    timeSinceLastAttack = 0;
                    PlayerHealth?.TakeDamage(Damage, 0, new Vector3(0, 0));
                }
            }
        }
    }
}