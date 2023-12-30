using ScriptableObjectNS.Weapon;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WeaponManagement;

namespace HudNS.Weapon.MeleeWeapn
{
    [RequireComponent(typeof(WeaponManagement.WeaponManager))]
    public class DurabilityShower : MonoBehaviour
    {
        public Image MeleeDurabilityLeft_MainLine;
        public Image MeleeDurabilityLeft_Background;
        [Tooltip("How much time for durability hud to sync with the weapon durability.")]
        public float FillTime = 0.5f;
        [Tooltip("How much time for durability hud to appear.")]
        public float AlphaTimeAppear = 0.1f;
        [Tooltip("How much time for durability hud to disappear.")]
        public float AlphaTimeDisappear = 5f;
        protected ActiveWeapon activeWeapon;
        private WeaponManager weaponManager;
        private float startedMainLineAlpha;
        private float startedBackgroundAlpha;
        private Coroutine currentCoroutine = null;
        private void Start()
        {
            startedMainLineAlpha = MeleeDurabilityLeft_MainLine.color.a;
            startedBackgroundAlpha = MeleeDurabilityLeft_Background.color.a;
            MeleeDurabilityLeft_MainLine.color = new Color(MeleeDurabilityLeft_MainLine.color.r, MeleeDurabilityLeft_MainLine.color.g, MeleeDurabilityLeft_MainLine.color.b, 0);
            MeleeDurabilityLeft_Background.color = new Color(MeleeDurabilityLeft_Background.color.r, MeleeDurabilityLeft_Background.color.g, MeleeDurabilityLeft_Background.color.b, 0);
            weaponManager = GetComponent<WeaponManager>();
            activeWeapon = weaponManager.ActiveWeapon;
            weaponManager.OnWeaponChange += ShowCurrentDurabilityOfActiveMelee_WithoutAppearing;
            //PlayerBattleInput.ReloadInputStarted += ShowCurrentDurabilityOfActiveMelee;
            ShowCurrentDurabilityOfActiveMelee_WithoutAppearing();
        }
        private void OnDisable()
        {
            weaponManager.OnWeaponChange -= ShowCurrentDurabilityOfActiveMelee_WithoutAppearing;
            //PlayerBattleInput.ReloadInputStarted -= ShowCurrentDurabilityOfActiveMelee;
        }
        public void ShowCurrentDurabilityOfActiveMelee()
        {
            if (weaponManager.GetCurrentSelectedWeapon().WeaponData.WeaponType == WeaponNS.WeaponType.Melee)
            {
                MeleeData meleeData = weaponManager.GetCurrentSelectedWeapon().WeaponData as MeleeData;
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                    currentCoroutine = null;
                }
                currentCoroutine = StartCoroutine(ShowDurability((float)meleeData.CurrentDurability / meleeData.MaxDurability, FillTime, AlphaTimeAppear, AlphaTimeDisappear));
            }
        }
        public void ShowCurrentDurabilityOfActiveMelee_WithoutAppearing()
        {
            if (weaponManager.GetCurrentSelectedWeapon()?.WeaponData.WeaponType == WeaponNS.WeaponType.Melee)
            {
                MeleeData meleeData = weaponManager.GetCurrentSelectedWeapon().WeaponData as MeleeData;
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                    currentCoroutine = null;
                }
                float durabirity = (float)meleeData.CurrentDurability / meleeData.MaxDurability;
                MeleeDurabilityLeft_MainLine.fillAmount = durabirity;
                MeleeDurabilityLeft_MainLine.color = new Color(MeleeDurabilityLeft_MainLine.color.r, MeleeDurabilityLeft_MainLine.color.g, MeleeDurabilityLeft_MainLine.color.b, startedMainLineAlpha);
                MeleeDurabilityLeft_Background.color = new Color(MeleeDurabilityLeft_Background.color.r, MeleeDurabilityLeft_Background.color.g, MeleeDurabilityLeft_Background.color.b, startedBackgroundAlpha);
                currentCoroutine = StartCoroutine(ShowDurability(durabirity, 0.000001f, 0.000001f, AlphaTimeDisappear));
            }
        }
        protected IEnumerator ShowDurability(float durability, float fillTime, float alphaTimeAppear, float alphaTimeDisappear)
        {
            float time = 0, currentAlpha_MainLine = MeleeDurabilityLeft_MainLine.color.a, currentAlpha_Background = MeleeDurabilityLeft_MainLine.color.a;
            while (Mathf.Abs(MeleeDurabilityLeft_MainLine.color.a - startedMainLineAlpha) >= 0.001 || Mathf.Abs(MeleeDurabilityLeft_Background.color.a - startedBackgroundAlpha) >= 0.001)
            {
                currentAlpha_MainLine = Mathf.Lerp(currentAlpha_MainLine, startedMainLineAlpha, time / alphaTimeAppear);
                currentAlpha_Background = Mathf.Lerp(currentAlpha_Background, startedBackgroundAlpha, time / alphaTimeAppear);
                MeleeDurabilityLeft_MainLine.color = new Color(MeleeDurabilityLeft_MainLine.color.r, MeleeDurabilityLeft_MainLine.color.g, MeleeDurabilityLeft_MainLine.color.b, currentAlpha_MainLine);
                MeleeDurabilityLeft_Background.color = new Color(MeleeDurabilityLeft_Background.color.r, MeleeDurabilityLeft_Background.color.g, MeleeDurabilityLeft_Background.color.b, currentAlpha_Background);
                time += Time.deltaTime;
                yield return null;
            }
            MeleeDurabilityLeft_MainLine.color = new Color(MeleeDurabilityLeft_MainLine.color.r, MeleeDurabilityLeft_MainLine.color.g, MeleeDurabilityLeft_MainLine.color.b, startedMainLineAlpha);
            MeleeDurabilityLeft_Background.color = new Color(MeleeDurabilityLeft_Background.color.r, MeleeDurabilityLeft_Background.color.g, MeleeDurabilityLeft_Background.color.b, startedBackgroundAlpha);
            time = 0;
            while (Mathf.Abs(MeleeDurabilityLeft_MainLine.fillAmount - durability) >= 0.001)
            {
                MeleeDurabilityLeft_MainLine.fillAmount = Mathf.Lerp(MeleeDurabilityLeft_MainLine.fillAmount, durability, time / fillTime);
                time += Time.deltaTime;
                yield return null;
            }
            MeleeDurabilityLeft_MainLine.fillAmount = durability;
            time = 0; currentAlpha_MainLine = startedMainLineAlpha; currentAlpha_Background = startedBackgroundAlpha;
            while (MeleeDurabilityLeft_MainLine.color.a >= 0.001 || MeleeDurabilityLeft_Background.color.a >= 0.001)
            {
                currentAlpha_MainLine = Mathf.Lerp(currentAlpha_MainLine, 0, time / alphaTimeDisappear);
                currentAlpha_Background = Mathf.Lerp(currentAlpha_Background, 0, time / alphaTimeDisappear);
                MeleeDurabilityLeft_MainLine.color = new Color(MeleeDurabilityLeft_MainLine.color.r, MeleeDurabilityLeft_MainLine.color.g, MeleeDurabilityLeft_MainLine.color.b, currentAlpha_MainLine);
                MeleeDurabilityLeft_Background.color = new Color(MeleeDurabilityLeft_Background.color.r, MeleeDurabilityLeft_Background.color.g, MeleeDurabilityLeft_Background.color.b, currentAlpha_Background);
                time += Time.deltaTime;
                yield return null;
            }
            MeleeDurabilityLeft_MainLine.color = new Color(MeleeDurabilityLeft_MainLine.color.r, MeleeDurabilityLeft_MainLine.color.g, MeleeDurabilityLeft_MainLine.color.b, 0);
            MeleeDurabilityLeft_Background.color = new Color(MeleeDurabilityLeft_Background.color.r, MeleeDurabilityLeft_Background.color.g, MeleeDurabilityLeft_Background.color.b, 0);
            currentCoroutine = null;
        }
    }
}