﻿// - AUTHOR : Pavel Cristian.
// - WHERE SHOULD BE ATTACHED : This script should be attached on the main root of the character, 
//	 on the GameObject the Rigidbody / CharacterController script is attached.
// - PURPOSE OF THE SCRIPT : The purpose of this script is to gather data from the ground below the character and use the
//   data to find a user-defined sound for the type of ground found.

// DISCLAIMER : THIS SCRIPT CAN BE USED IN ANY WAY, MENTIONING MY WORK WILL BE GREATLY APPRECIATED BUT NOT REQUIRED.

using PlayerScriptsNS;
using SoundNS;
using UnityEngine;
using static SettingsNS.AudioSettings;

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
    public class CharacterFootsteps : AudioSetup
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
        [SerializeField] AudioKind audioKind;
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
        float lastPlayTime;
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
            availableSources.Add(new AudioObject(audioSource, audioSource.volume, audioKind));
            if (errorMessage != "")
            {
                Debug.LogError(errorMessage);
                enabled = false;
            }
        }
        private void Update()
        {
            CheckGround();
            if (triggeredBy == TriggeredBy.TRAVELED_DISTANCE)
            {
                float speed = ((characterController ? characterController.GetCharacterVelocity() : characterRigidbody.velocity) * Time.deltaTime).magnitude;
                if (isGrounded)
                {
                    // Advance the step cycle only if the character is grounded.
                    AdvanceStepCycle(speed);
                }
            }
        }
        public void TryPlayFootstep()
        {
            if (isGrounded)
                PlayFootstep();
        }
        private void PlayLandSound() =>
            audioSource.PlayOneShot(SurfaceManager.singleton.GetLandsound(currentGroundInfo.collider, currentGroundInfo.point), landVolume * GetVolumeOfType(audioKind));
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
            AudioClip randomFootstep = SurfaceManager.singleton.GetFootstep(currentGroundInfo.collider, currentGroundInfo.point);
            float randomVolume = Random.Range(minVolume, maxVolume);
            if (randomFootstep)
                audioSource.PlayOneShot(randomFootstep, randomVolume * GetVolumeOfType(audioKind));
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
            if (!previouslyGrounded && isGrounded)
                PlayLandSound();
            // print(isGrounded);
        }
    }
}
