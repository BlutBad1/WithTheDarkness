using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameObjectsControllingNS
{
	public class ObjectShift : MonoBehaviour
	{
		[SerializeField, FormerlySerializedAs("GameObject")]
		protected GameObject gameObjectToShift;
		[SerializeField, FormerlySerializedAs("GameObject")]
		protected Vector3 startingPosition;
		[SerializeField, FormerlySerializedAs("GameObject")]
		protected Quaternion startingRotation;
		[SerializeField, FormerlySerializedAs("GameObject")]
		protected Vector3 endingPosition;
		[SerializeField, FormerlySerializedAs("GameObject")]
		protected Quaternion endingRotation;
		[SerializeField, FormerlySerializedAs("GameObject")]
		protected float shiftPositionDuration = 100f;
		[SerializeField, FormerlySerializedAs("GameObject")]
		protected float shiftRotationDuration = 100f;

		protected Coroutine currentCoroutine;

		protected virtual void Awake()
		{
			if (!gameObjectToShift)
				gameObjectToShift = this.gameObject;
		}
		public virtual void ShiftingToEndingTransform()
		{
			if (currentCoroutine != null)
				StopCoroutine(currentCoroutine);
			currentCoroutine = StartCoroutine(ShiftTo(endingPosition, endingRotation));
		}
		public virtual void ShiftingToStartingTransform()
		{
			if (currentCoroutine != null)
				StopCoroutine(currentCoroutine);
			currentCoroutine = StartCoroutine(ShiftTo(startingPosition, startingRotation));
		}
		protected virtual IEnumerator ShiftTo(Vector3 shitPosition, Quaternion shitRotation)
		{
			gameObjectToShift.transform.DOLocalMove(shitPosition, shiftPositionDuration).SetUpdate(UpdateType.Normal, true);
			gameObjectToShift.transform.DOLocalRotate(shitRotation.eulerAngles, shiftRotationDuration).SetUpdate(UpdateType.Normal, true);
			yield return null;
		}
	}
}