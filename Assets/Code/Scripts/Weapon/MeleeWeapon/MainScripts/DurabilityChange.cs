using HudNS.Weapon.MeleeWeapn;
using ScriptableObjectNS.Weapon;
using SoundNS;
using UnityEngine;
namespace WeaponNS.MeleeWeaponNS
{
    public class DurabilityChange : MonoBehaviour
    {
        public GameObject WeaponGameObject;
        public MeleeData MeleeData;
        public ActiveWeapon ActiveWeapon;
        public DurabilityShower DurabilityShower;
        public AudioSourceManager BreakSound; 
        public ParticleSystem DurabilityEndParticleSystem;
        public void OnDurabilityEnd()
        {
            WeaponGameObject.SetActive(false);
            ActiveWeapon.ActiveWeapons.ActiveMeleeWeapon = null;
            DurabilityEndParticleSystem.Play();
            BreakSound.PlayAudioSource();
        }
        public void OnDurabilityDecrease()
        {
            DurabilityShower.ShowCurrentDurabilityOfActiveMelee();
        }
    }
}