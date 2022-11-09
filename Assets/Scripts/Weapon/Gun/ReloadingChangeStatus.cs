
using UnityEngine;

public class ReloadingChangeStatus : MonoBehaviour
{
    [SerializeField] GunData gunData;

    public void ChangeWeaponReloadStatus()
    {
        gunData.reloading = !gunData.reloading;
    }
}
