using System.Collections;
using UnityEngine;

namespace WeaponNS.ShootingWeaponNS.AnimationsNS
{
    public class CylinderRoll : MonoBehaviour
    {
        [SerializeField]
        private GameObject cylinder;
        [SerializeField]
        private float rollTime = 0.1f;
        private bool rotating;
        private bool startCylinderRoll;
        [SerializeField]
        private Vector3 targetAxis;
        void Update()
        {
            if (startCylinderRoll & !rotating)
            {
                startCylinderRoll = false;
                StartCoroutine(CylinderRotation());
            }
        }
        public void CylinderRollAnim() =>
            startCylinderRoll = true;
        private IEnumerator CylinderRotation()//Don't use as an AnimationEvent 
        {
            rotating = true;
            float timeElapsed = 0;
            Quaternion startRotation = cylinder.transform.localRotation;
            Quaternion targetRotation = cylinder.transform.localRotation * Quaternion.Euler(targetAxis);
            while (timeElapsed < rollTime)
            {
                cylinder.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / rollTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            cylinder.transform.localRotation = targetRotation;
            rotating = false;
        }
        public void CylinderRestartPos() =>
            cylinder.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}
