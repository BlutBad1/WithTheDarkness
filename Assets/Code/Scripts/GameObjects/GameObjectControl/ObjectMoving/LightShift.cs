using MyConstants;
using System.Collections;
using UnityEngine;
using WeaponManagement;

namespace GameObjectsControllingNS
{
    public class LightShift : ObjectShift
    {
        private GameObject weaponHolder;
        protected override void Awake()
        {
            if (!GameObject)
                GameObject = GameObject.Find(CommonConstants.PIVOT_MAIN_LIGHT);
#if UNITY_EDITOR
            if (!GameObject)
                Debug.LogError("Light is not found!");
#endif
            weaponHolder = GameObject.Find(CommonConstants.WEAPON_HOLDER);
        }
        private void OnEnable()
        {
            if (weaponHolder.activeInHierarchy && weaponHolder.TryGetComponent(out WeaponManager wM))
                wM.StopCoroutine("ShiftTo");
            ShiftingToEndingTransform();
        }
        private void OnDisable()
        {
            if (weaponHolder.activeInHierarchy && weaponHolder.TryGetComponent(out WeaponManager wM))
                wM.StartCoroutine(DisableCheck(wM));
        }
        IEnumerator DisableCheck(WeaponManager wM)
        {
            wM.StopCoroutine("ShiftTo");
            if (wM.ActiveWeapon.CurrentSelectedActiveWeapon == null || wM.ActiveWeapon.CurrentSelectedActiveWeapon.WeaponData.IsTwoHanded)
                yield return new WaitForSeconds(0.1f);
            if (wM.ActiveWeapon.CurrentSelectedActiveWeapon == null || !wM.ActiveWeapon.CurrentSelectedActiveWeapon.WeaponData.IsTwoHanded)
                wM.StartCoroutine(ShiftTo(StartingPosition, StartingRotation));
        }
    }
}