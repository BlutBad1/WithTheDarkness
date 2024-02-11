using DamageableNS;
using PlayerScriptsNS;
using UnityEngine;
namespace LightNS.Player
{
	public class PlayerLightExhaustion : MonoBehaviour
	{
		public LightGlowTimer LightGlowTimer;
		public float Damage = 20f;
		public float TimeBetweenAttacks = 5f;
		public PlayerHealth PlayerHealth;
		[HideInInspector]
		public bool IsExhastionEnabled = false;
		private float timeSinceLastAttack = 0f;
		private void Start()
		{
			if (!LightGlowTimer)
				LightGlowTimer = UtilitiesNS.Utilities.GetComponentFromGameObject<LightGlowTimer>(gameObject);
		}
		private void Update()
		{
			if (LightGlowTimer.CurrentTimeLeftToGlow <= 0)
				IsExhastionEnabled = true;
			LightExhastion();
		}
		public void LightExhastion()
		{
			if (IsExhastionEnabled)
			{
				timeSinceLastAttack += Time.deltaTime;
				if (timeSinceLastAttack > TimeBetweenAttacks)
				{
					timeSinceLastAttack = 0;
					PlayerHealth?.TakeDamage(new TakeDamageData(PlayerHealth, Damage, 0, null, null));
				}
			}
			else
				timeSinceLastAttack = 0;
		}
	}
}