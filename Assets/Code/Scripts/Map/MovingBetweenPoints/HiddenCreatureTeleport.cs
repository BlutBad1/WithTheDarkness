using CreatureNS;
using ExtensionMethods;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class HiddenCreatureTeleport : MonoBehaviour
{
	[SerializeField, FormerlySerializedAs("OriginalTransformRoot")]
	protected Transform originalTransformRoot;
	[SerializeField, FormerlySerializedAs("DestinationTransformRoot")]
	protected Transform destinationTransformRoot;
	[SerializeField, FormerlySerializedAs("TeleportLayers")]
	protected LayerMask teleportLayers;
	[SerializeField, FormerlySerializedAs("IsTrigger")]
	protected bool isTrigger = true;

	private Coroutine currentCoroutine;

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (CheckIfGameObjectMatchesConditions(other.gameObject))
			TeleportObject(other.gameObject);
	}
	public virtual void TeleportObject(GameObject go)
	{
		if (currentCoroutine != null)
			StopCoroutine(currentCoroutine);
		currentCoroutine = StartCoroutine(Teleport(go));
	}
	protected bool CheckIfGameObjectMatchesConditions(GameObject other) =>
		 isTrigger && teleportLayers.CheckIfLayerInLayerMask(other.layer);
	protected virtual IEnumerator Teleport(GameObject go)
	{
		ICreature creature = UtilitiesNS.Utilities.GetComponentFromGameObject<ICreature>(go);
		if (creature != null)
		{
			Vector3 originalPosition = go.transform.position;
			Quaternion originalRotation = go.transform.rotation;
			Quaternion relativeRotation = destinationTransformRoot.rotation * Quaternion.Inverse(originalTransformRoot.rotation);
			creature.SetPositionAndRotation(destinationTransformRoot.TransformPoint(originalTransformRoot.InverseTransformPoint(originalPosition)), relativeRotation * originalRotation);
			yield return new WaitForSeconds(0.005f);
		}
		currentCoroutine = null;
	}
}
