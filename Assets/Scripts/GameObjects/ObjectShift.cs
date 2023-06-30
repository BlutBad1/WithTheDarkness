using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace GameObjectsControllingNS
{
    public class ObjectShift : MonoBehaviour
    {
        public GameObject GameObject;
        public Vector3 StartingPosition;
        public Quaternion StartingRotation;
        public Vector3 EndingPosition;
        public Quaternion EndingRotation;
        public float ShiftPositionDuration = 100f;
        public float ShiftRotationDuration = 100f;
        protected Coroutine currentCoroutine;
        virtual protected void Awake()
        {
            if (!GameObject)
                GameObject = this.gameObject;
        }
        virtual public void ShiftingToEndingTransform()
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(ShiftTo(EndingPosition, EndingRotation));
        }
        virtual public void ShiftingToStartingTransform()
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(ShiftTo(StartingPosition, StartingRotation));
        }
        virtual protected IEnumerator ShiftTo(Vector3 shitPosition, Quaternion shitRotation)
        {
            GameObject.transform.DOLocalMove(shitPosition, ShiftPositionDuration).SetUpdate(UpdateType.Normal, true);
            GameObject.transform.DOLocalRotate(shitRotation.eulerAngles, ShiftRotationDuration).SetUpdate(UpdateType.Normal, true);
            yield return null;
        }
    }
}