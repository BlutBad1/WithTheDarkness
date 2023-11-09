using CreatureNS;
using System.Collections;
using UnityEngine;

public class HiddenPlayerTeleport : MonoBehaviour
{
    public Transform OriginalTransformRoot;
    public Transform DestinationTransformRoot;
    public LayerMask TeleportLayers;
    private Coroutine currentCoroutine;
    private void OnTriggerExit(Collider other)
    {
        if (TeleportLayers == (TeleportLayers | (1 << other.gameObject.layer)))
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(TeleportObject(other.gameObject));
        }
    }
    private IEnumerator TeleportObject(GameObject go)
    {
        ICreature creature = UtilitiesNS.Utilities.GetComponentFromGameObject<ICreature>(go);
        if (creature != null)
        {
            creature.BlockMovement();
            //Position
            Vector3 originalPosition = go.transform.position;
            go.transform.position = DestinationTransformRoot.TransformPoint(OriginalTransformRoot.InverseTransformPoint(originalPosition));
            //Rotation
            Quaternion originalRotation = go.transform.rotation;
            Quaternion relativeRotation = DestinationTransformRoot.rotation * Quaternion.Inverse(OriginalTransformRoot.rotation);
            go.transform.rotation = relativeRotation * originalRotation;
            yield return new WaitForSeconds(0.018f);
            creature.UnBlockMovement();
        }
        currentCoroutine = null;
    }
}
