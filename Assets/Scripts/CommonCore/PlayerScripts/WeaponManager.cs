using MyConstants.WeaponConstants;
using System;
using System.Collections;
using UnityEngine;

namespace WeaponManagement
{
    [System.Serializable]
    public class Weapon
    {
        public string Name;
        public GameObject WeaponGameObject;
        public bool IsTwoHanded = false;
        public bool IsUnlocked;
    }
    public class WeaponManager : MonoBehaviour
    {
        public GameObject Lamp;
        [SerializeField]
        public Weapon[] Weapons;
        int currentSelection = 0;
        private void Start()
        {
            if (!Lamp)
                Lamp = GameObject.Find(WeaponConstants.LAMP);
            if (Weapons == null)
                Weapons = new Weapon[0];
        }
        public void ChangeWeaponLockStatus(string name, bool newStatus) =>
            ChangeWeaponLockStatus(Array.FindIndex(Weapons, weapon => weapon.Name == name), newStatus);
        public void ChangeWeaponLockStatus(int index, bool newStatus)
        {
            if (Weapons?.Length >= index + 1)
                Weapons[index].IsUnlocked = newStatus;
#if UNITY_EDITOR
            else
                Debug.LogWarning("Weapon is not found!");
#endif
        }
        public void ChangeWeaponSelection(int index)
        {
            if (Weapons?.Length >= index + 1 && currentSelection != index)
            {
                if (Weapons[index].IsUnlocked)
                    StartCoroutine(ChangeWeaponCoroutine(index));
            }
        }
        IEnumerator ChangeWeaponCoroutine(int index)
        {
            Animator lampAnimator = Lamp.GetComponent<Animator>(), currentWeaponAnimator = Weapons[currentSelection].WeaponGameObject.GetComponent<Animator>();
            currentWeaponAnimator.SetTrigger(WeaponConstants.PUTTING_DOWN);
            if (Weapons[index].IsTwoHanded && Lamp.activeInHierarchy)
            {
                lampAnimator.SetTrigger(WeaponConstants.PUTTING_DOWN);
                while (!lampAnimator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN))
                    yield return null;
            }
            while (!currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN))
                yield return null;
            while ((lampAnimator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN) && lampAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                || (currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName(WeaponConstants.PUTTING_DOWN) && currentWeaponAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f))
            {
                yield return new WaitForSeconds(0.1f);
            }
            if (Weapons[index].IsTwoHanded)
                Lamp.SetActive(false);
            else
                Lamp.SetActive(true);
            Weapons[currentSelection].WeaponGameObject.SetActive(false);
            Weapons[index].WeaponGameObject.SetActive(true);
            currentSelection = index;
        }
    }
}