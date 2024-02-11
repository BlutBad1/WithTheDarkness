using System.Collections;
using UnityEngine;
using WeaponManagement;

namespace GameObjectsControllingNS
{
	public class LightShift : ObjectShift
	{
		[SerializeField]
		private WeaponManager weaponManager;

		private void OnEnable()
		{
			if (weaponManager.enabled)
				weaponManager.StopCoroutine("ShiftTo");
			ShiftingToEndingTransform();
		}
		private void OnDisable()
		{
			if (weaponManager.enabled)
				weaponManager.StartCoroutine(DisableCheck(weaponManager));
		}
		private IEnumerator DisableCheck(WeaponManager wM)
		{
			wM.StopCoroutine("ShiftTo");
			if (wM.ActiveWeapon.CurrentSelectedActiveWeapon == null || wM.ActiveWeapon.CurrentSelectedActiveWeapon.WeaponData.IsTwoHanded)
				yield return new WaitForSeconds(0.1f);
			if (wM.ActiveWeapon.CurrentSelectedActiveWeapon == null || !wM.ActiveWeapon.CurrentSelectedActiveWeapon.WeaponData.IsTwoHanded)
				wM.StartCoroutine(ShiftTo(startingPosition, startingRotation));
		}
	}
}