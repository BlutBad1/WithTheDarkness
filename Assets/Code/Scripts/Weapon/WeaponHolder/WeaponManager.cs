using MyConstants.WeaponConstants;
using ScriptableObjectNS.Weapon;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using WeaponNS;
using static Codice.Client.Common.Connection.AskCredentialsToUser;

namespace WeaponManagement
{
    [System.Serializable]
    public class Weapon
    {
        public WeaponData WeaponData;
        public GameObject WeaponGameObject;
    }
    public class WeaponManager : MonoBehaviour
    {
        [Header("Lamp")]
        public GameObject MainSpotLight;
        public GameObject Lamp;
        [Header("All Weapon")]
        public Weapon[] Weapons;
        [Header("Active Weapon")]
        public ActiveWeapon ActiveWeapon;
        //null is default state without any active weapon to use
        public Action OnWeaponChange;
        public bool UseLampAsDefault = true;
        private void Awake()
        {
            if (!Lamp)
                Lamp = GameObject.Find(LampConstants.LAMP);
            if (Weapons == null)
                Weapons = new Weapon[0];
            DefineSelection();
        }
        public Weapon GetCurrentSelectedWeapon() =>
           ActiveWeapon.CurrentSelectedActiveWeapon;
        public void ChangeActiveWeapon(WeaponType weaponType, WeaponData weaponData)
        {
            Weapon weapon = Weapons.First(x => x.WeaponData == weaponData);
            if (weapon != null)
            {
                switch (weaponType)
                {
                    case WeaponType.Melee:
                        ActiveWeapon.ActiveWeapons.ActiveMeleeWeapon = weapon.WeaponData;
                        break;
                    case WeaponType.OneHandedGun:
                        ActiveWeapon.ActiveWeapons.ActiveOneHandedGun = weapon.WeaponData;
                        break;
                    case WeaponType.TwoHandedGun:
                        ActiveWeapon.ActiveWeapons.ActiveTwoHandedGun = weapon.WeaponData;
                        break;
                    default:
                        break;
                }
                if (ActiveWeapon.CurrentSelectedActiveWeapon == null || ActiveWeapon.CurrentSelectedActiveWeapon.WeaponData.WeaponType == weaponType)
                    ChangeWeaponSelection(weapon);
            }
#if UNITY_EDITOR
            else
                Debug.LogWarning("Weapon is not found!");
#endif
        }
        public bool IsActiveWeapon(WeaponData weaponData)
        {
            if (weaponData == ActiveWeapon.ActiveWeapons.ActiveMeleeWeapon || weaponData == ActiveWeapon.ActiveWeapons.ActiveOneHandedGun || weaponData == ActiveWeapon.ActiveWeapons.ActiveTwoHandedGun) return true;
            return false;
        }
        public void ChangeWeaponSelection(WeaponType weaponType)
        {
            Weapon newWeapon = Weapons.FirstOrDefault(x => x.WeaponData.WeaponType == weaponType && IsActiveWeapon(x.WeaponData));
            ChangeWeaponSelection(newWeapon);
        }
        public void ChangeWeaponSelection(Weapon newWeapon)
        {
            if (ActiveWeapon.CurrentSelectedActiveWeapon != newWeapon && newWeapon != null)
                StartCoroutine(ChangeWeaponCoroutine(newWeapon));
        }
        private void DefineSelection()
        {
            foreach (var item in Array.FindAll(Weapons, weapon => weapon.WeaponGameObject.activeInHierarchy && !IsActiveWeapon(weapon.WeaponData)))
                item.WeaponGameObject.SetActive(false);
            Weapon[] activeWeapon = Array.FindAll(Weapons, weapon => IsActiveWeapon(weapon.WeaponData));
            if (activeWeapon != null && activeWeapon.Length > 0)
            {
                ActiveWeapon.CurrentSelectedActiveWeapon = Array.Find(Weapons, weapon => ActiveWeapon.CurrentSelectedActiveWeapon == activeWeapon[0]) == null
                    ? Array.Find(Weapons, weapon => weapon == activeWeapon[0])
                    : ActiveWeapon.CurrentSelectedActiveWeapon;
                if (activeWeapon?.Length > 1)
                {
                    for (int i = 0; i < activeWeapon.Length; i++)
                    {
                        if (activeWeapon[i] != ActiveWeapon.CurrentSelectedActiveWeapon)
                            activeWeapon[i].WeaponGameObject.SetActive(false);
                    }
                }
            }
            else
                ActiveWeapon.CurrentSelectedActiveWeapon = null;
            if (ActiveWeapon.CurrentSelectedActiveWeapon != null)
            {
                ActiveWeapon.CurrentSelectedActiveWeapon.WeaponGameObject.SetActive(true);
                if (ActiveWeapon.CurrentSelectedActiveWeapon.WeaponData.IsTwoHanded)
                    Lamp.SetActive(false);
                else
                    Lamp.SetActive(true);
            }
            else
                Lamp.SetActive(UseLampAsDefault);
        }
        private IEnumerator ChangeWeaponCoroutine(Weapon newWeapon)
        {
            Animator lampAnimator = Lamp.GetComponent<Animator>(), currentWeaponAnimator = ActiveWeapon.CurrentSelectedActiveWeapon?.WeaponGameObject.GetComponent<Animator>();
            if (ActiveWeapon.CurrentSelectedActiveWeapon != null)
                currentWeaponAnimator.SetTrigger(MainWeaponConstants.PUTTING_DOWN);
            if (newWeapon.WeaponData.IsTwoHanded && Lamp.activeInHierarchy)
            {
                lampAnimator.SetTrigger(MainWeaponConstants.PUTTING_DOWN);
                while (!lampAnimator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN))
                    yield return null;
            }
            while (ActiveWeapon.CurrentSelectedActiveWeapon != null && !currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN))
                yield return null;
            while ((lampAnimator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN) && lampAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                || (ActiveWeapon.CurrentSelectedActiveWeapon != null && currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN) && currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f))
            {
                yield return new WaitForSeconds(0.1f);
            }
            if (newWeapon.WeaponData.IsTwoHanded)
                Lamp.SetActive(false);
            else
                Lamp.SetActive(true);
            if (ActiveWeapon.CurrentSelectedActiveWeapon != null)
                ActiveWeapon.CurrentSelectedActiveWeapon.WeaponGameObject.SetActive(false);
            ActiveWeapon.CurrentSelectedActiveWeapon = newWeapon;
            newWeapon.WeaponGameObject.SetActive(true);
            OnWeaponChange?.Invoke();
        }
    }
}