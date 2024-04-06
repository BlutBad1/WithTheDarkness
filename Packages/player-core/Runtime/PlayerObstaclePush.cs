using UnityEngine;
namespace PlayerScriptsNS
{
    public class PlayerObstaclePush : MonoBehaviour
    {
        [SerializeField]
        private float forceMagnitude;

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody rigidbody = hit.collider.attachedRigidbody;
            if (rigidbody)
            {
                Vector3 fromDirection = hit.gameObject.transform.position - transform.position;
                fromDirection.y = 0;
                fromDirection.Normalize();
                rigidbody.AddForceAtPosition(fromDirection * forceMagnitude, transform.position, ForceMode.Impulse);
            }
        }
    }
}
