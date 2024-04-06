using PlayerScriptsNS;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Footsteps
{
    public enum TriggeredBy
    {
        COLLISION_DETECTION,    // The footstep sound will be played when the physical foot collides with the ground.
        TRAVELED_DISTANCE       // The footstep sound will be played after the character has traveled a certain distance
    }
    public enum ControllerType
    {
        RIGIDBODY,
        CHARACTER_CONTROLLER
    }
    public class CharacterFootsteps : MonoBehaviour
    {
        [Tooltip("The method of triggering footsteps.")]
        [SerializeField] TriggeredBy triggeredBy;
        [Tooltip("This is used to determine what distance has to be traveled in order to play the footstep sound.")]
        [SerializeField] float distanceBetweenSteps = 1.8f;
        [Tooltip("To know how much the character moved, a reference to a rigidbody / character controller is needed.")]
        [SerializeField] ControllerType controllerType;
        [SerializeField] Rigidbody characterRigidbody;
        [SerializeField] PlayerMotor characterController;//	[SerializeField] CharacterController characterController;
        [Tooltip("You need an audio source to play a footstep sound.")]
        [SerializeField] AudioSource audioSource;
        // Random volume between this limits
        [SerializeField] float minVolume = 0.3f;
        [SerializeField] float maxVolume = 0.5f;
        [SerializeField] float landVolume = 0.5f;
        [Tooltip("If this is enabled, you can see how far the script will check for ground, and the radius of the check.")]
        [SerializeField] bool debugMode = true;
        [Tooltip("How high, relative to the character's pivot point the start of the ray is.")]
        [SerializeField] float groundCheckHeight = 0.5f;
        [Tooltip("What is the radius of the ray.")]
        [SerializeField] float groundCheckRadius = 0.5f;
        [Tooltip("How far the ray is casted.")]
        [SerializeField] float groundCheckDistance = 0.3f;
        [Tooltip("What are the layers that should be taken into account when checking for ground.")]
        [SerializeField] LayerMask groundLayers;
        Transform thisTransform;
        RaycastHit currentGroundInfo;
        float stepCycleProgress;
        bool previouslyGrounded;
        bool isGrounded = true;
        private void Start()
        {
            if (groundLayers.value == 0)
                groundLayers = 1;
            thisTransform = transform;
            string errorMessage = "";

            if (!audioSource) errorMessage = "No audio source assigned in the inspector, footsteps cannot be played";
            else if (triggeredBy == TriggeredBy.TRAVELED_DISTANCE && !characterRigidbody && !characterController) errorMessage = "Please assign a Rigidbody or CharacterController component in the inspector, footsteps cannot be played";
            else if (!FindObjectOfType<SurfaceManager>()) errorMessage = "Please create a Footstep Database, otherwise footsteps cannot be played, you can create a database" +
                                                                        " by clicking 'FootstepsCreator' in the main menu";
            if (errorMessage != "")
            {
                Debug.LogError(errorMessage);
                enabled = false;
            }
        }
        private void Update()
        {
            CheckGround();
            PlayLandSound();
            if (!isGrounded)
                stepCycleProgress = 0f;
            else if (previouslyGrounded && triggeredBy == TriggeredBy.TRAVELED_DISTANCE)
            {
                float speed = ((characterController ? characterController.Character.velocity : characterRigidbody.velocity) * Time.deltaTime).magnitude;
                AdvanceStepCycle(speed);
            }
        }
        public void TryPlayFootstep()
        {
            if (isGrounded)
                PlayFootstep();
        }
        private void PlayLandSound()
        {
            if (!previouslyGrounded && isGrounded)
            {
                Dictionary<AudioClip, float> landSound = SurfaceManager.singleton.GetLandsound(currentGroundInfo.collider, currentGroundInfo.point);
                if (landSound != null)
                    audioSource.PlayOneShot(landSound.Keys.First(), landSound.Values.First() * landVolume);
            }
        }
        private void AdvanceStepCycle(float increment)
        {
            stepCycleProgress += increment;
            if (stepCycleProgress > distanceBetweenSteps)
            {
                stepCycleProgress = 0f;
                PlayFootstep();
            }
        }
        private void PlayFootstep()
        {
            Dictionary<AudioClip, float> footstepSound = SurfaceManager.singleton.GetFootstep(currentGroundInfo.collider, currentGroundInfo.point);
            float randomVolume = Random.Range(minVolume, maxVolume);
            if (footstepSound != null)
                audioSource.PlayOneShot(footstepSound.Keys.First(), randomVolume * footstepSound.Values.First());
        }
        private void OnDrawGizmos()
        {
            if (debugMode)
            {
                Gizmos.DrawWireSphere(transform.position + Vector3.up * groundCheckHeight, groundCheckRadius);
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position + Vector3.up * groundCheckHeight, Vector3.down * (groundCheckDistance + groundCheckRadius));
            }
        }
        private void CheckGround()
        {
            previouslyGrounded = isGrounded;
            Ray ray = new Ray(thisTransform.position + Vector3.up * groundCheckHeight, Vector3.down);
            if (Physics.SphereCast(ray, groundCheckRadius, out currentGroundInfo, groundCheckDistance, groundLayers, QueryTriggerInteraction.Ignore))
                isGrounded = true;
            else
                isGrounded = false;
        }
    }
}
