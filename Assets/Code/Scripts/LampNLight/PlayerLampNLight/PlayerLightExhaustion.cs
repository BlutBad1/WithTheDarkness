using DamageableNS;
using PlayerScriptsNS;
using UnityEngine;
using UnityEngine.Serialization;

namespace LightNS.Player
{
	public class PlayerLightExhaustion : MonoBehaviour
	{
		[SerializeField, FormerlySerializedAs("LightGlowTimer")]
		private LightGlowTimer lightGlowTimer;
		[SerializeField, FormerlySerializedAs("Damage")]
		private float damage = 20f;
		[SerializeField, FormerlySerializedAs("TimeBetweenAttacks")]
		private float timeBetweenAttacks = 5f;
		[SerializeField, FormerlySerializedAs("PlayerHealth")]
		private PlayerHealth playerHealth;

		private float timeSinceLastAttack = 0f;

		public bool IsExhastionEnabled { get; set; }
		public LightGlowTimer LightGlowTimer { get => lightGlowTimer; }

		private void Update()
		{
			if (LightGlowTimer.CurrentTimeLeftToGlow <= 0)
				IsExhastionEnabled = true;
			LightExhastion();
		}
		private void LightExhastion()
		{
			if (IsExhastionEnabled)
			{
				timeSinceLastAttack += Time.deltaTime;
				if (timeSinceLastAttack > timeBetweenAttacks)
				{
					timeSinceLastAttack = 0;
					playerHealth?.TakeDamage(new TakeDamageData(playerHealth, damage, 0, null, null));
				}
			}
			else
				timeSinceLastAttack = 0;
		}
	}
}