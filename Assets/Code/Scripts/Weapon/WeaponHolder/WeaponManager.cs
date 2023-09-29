using MyConstants.WeaponConstants;
using ScriptableObjectNS.Weapon;
using System;
using System.Collections;
using UnityEngine;
using WeaponNS;

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
        public GameObject Lamp;
        [SerializeField]
        public Weapon[] Weapons;
        [HideInInspector]
        //-1 is default state without any weapon to use
        public int currentSelection = -1;
        public delegate void WeaponChange();
        public WeaponChange OnWeaponChange;
        public bool UseLampAsDefault = true;
        private void Awake()
        {
            if (!Lamp)
                Lamp = GameObject.Find(LampConstants.LAMP);
            if (Weapons == null)
                Weapons = new Weapon[0];
            DefineSelection();
        }
        void DefineSelection()
        {
            foreach (var item in Array.FindAll(Weapons, weapon => weapon.WeaponGameObject.activeInHierarchy && !weapon.WeaponData.IsUnlocked))
                item.WeaponGameObject.SetActive(false);
            Weapon[] activeWeapon = Array.FindAll(Weapons, weapon => weapon.WeaponGameObject.activeInHierarchy);
            if (activeWeapon != null && activeWeapon?.Length > 0)
            {
                currentSelection = Array.FindIndex(Weapons, weapon => weapon == activeWeapon[0]);
                if (activeWeapon?.Length > 1)
                {
                    for (int i = 0; i < Weapons.Length; i++)
                    {
                        if (Weapons[i] != Weapons[currentSelection])
                            Weapons[i].WeaponGameObject.SetActive(false);
                    }
                }
            }
            else
                currentSelection = Array.FindIndex(Weapons, weapon => weapon.WeaponData.IsUnlocked == true);
            if (currentSelection != -1)
            {
                Weapons[currentSelection].WeaponGameObject.SetActive(true);
                if (Weapons[currentSelection].WeaponData.IsTwoHanded)
                    Lamp.SetActive(false);
                else
                    Lamp.SetActive(true);
            }
            else
                Lamp.SetActive(UseLampAsDefault);
        }
        public Weapon GetCurrentSelectedWeapon() =>
            currentSelection != -1 ? Weapons[currentSelection] : null;
        public void ChangeWeaponLockStatus(string name, bool newStatus) =>
            ChangeWeaponLockStatus(Array.FindIndex(Weapons, weapon => weapon.WeaponData.Name == name), newStatus);
        public void ChangeWeaponLockStatus(int index, bool newStatus)
        {
            if (Weapons?.Length >= index + 1)
            {
                Weapons[index].WeaponData.IsUnlocked = newStatus;
                if (currentSelection == -1)
                    ChangeWeaponSelection(index);
            }
#if UNITY_EDITOR
            else
                Debug.LogWarning("Weapon is not found!");
#endif
        }
        public void ChangeWeaponSelection(string name) =>
          ChangeWeaponSelection(Array.FindIndex(Weapons, weapon => weapon.WeaponData.Name == name));
        public void ChangeWeaponSelection(int index)
        {
            if (Weapons?.Length >= index + 1 && currentSelection != index)
            {
                if (Weapons[index].WeaponData.IsUnlocked)
                    StartCoroutine(ChangeWeaponCoroutine(index));
            }
        }
        IEnumerator ChangeWeaponCoroutine(int index)
        {
            Animator lampAnimator = Lamp.GetComponent<Animator>(), currentWeaponAnimator = Weapons[currentSelection == -1 ? index : currentSelection].WeaponGameObject.GetComponent<Animator>();
            if (currentSelection != -1)
                currentWeaponAnimator.SetTrigger(MainWeaponConstants.PUTTING_DOWN);
            if (Weapons[index].WeaponData.IsTwoHanded && Lamp.activeInHierarchy)
            {
                lampAnimator.SetTrigger(MainWeaponConstants.PUTTING_DOWN);
                while (!lampAnimator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN))
                    yield return null;
            }
            while (!currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN) && currentSelection != -1)
                yield return null;
            while ((lampAnimator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN) && lampAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                || (currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName(MainWeaponConstants.PUTTING_DOWN) && currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f))
            {
                yield return new WaitForSeconds(0.1f);
            }
            if (Weapons[index].WeaponData.IsTwoHanded)
                Lamp.SetActive(false);
            else
                Lamp.SetActive(true);
            if (currentSelection != -1)
                Weapons[currentSelection].WeaponGameObject.SetActive(false);
            Weapons[index].WeaponGameObject.SetActive(true);
            currentSelection = index;
            OnWeaponChange?.Invoke();
        }
    }
}