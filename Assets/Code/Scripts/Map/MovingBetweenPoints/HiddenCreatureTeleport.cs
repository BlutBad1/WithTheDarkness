using CreatureNS;
using ExtensionMethods;
using System.Collections;
using UnityEngine;

public class HiddenCreatureTeleport : MonoBehaviour
{
    public Transform OriginalTransformRoot;
    public Transform DestinationTransformRoot;
    public LayerMask TeleportLayers;
    public bool IsTrigger = true;
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
         IsTrigger && TeleportLayers.CheckIfLayerInLayerMask(other.layer);
    protected virtual IEnumerator Teleport(GameObject go)
    {
        ICreature creature = UtilitiesNS.Utilities.GetComponentFromGameObject<ICreature>(go);
        if (creature != null)
        {
            // creature.BlockMovement();
            //Position
            Vector3 originalPosition = go.transform.position;
            //go.transform.position = DestinationTransformRoot.TransformPoint(OriginalTransformRoot.InverseTransformPoint(originalPosition));
            //Rotation
            Quaternion originalRotation = go.transform.rotation;
            Quaternion relativeRotation = DestinationTransformRoot.rotation * Quaternion.Inverse(OriginalTransformRoot.rotation);
            //go.transform.rotation = relativeRotation * originalRotation;
            //Setting
            creature.SetPositionAndRotation(DestinationTransformRoot.TransformPoint(OriginalTransformRoot.InverseTransformPoint(originalPosition)), relativeRotation * originalRotation);
            yield return new WaitForSeconds(0.005f);
            //  creature.UnBlockMovement();
        }
        currentCoroutine = null;
    }
}
