using EnemyNS.Base;
using SoundNS;
using UnityEngine;
using UnityEngine.AI;
using static SettingsNS.AudioSettings;

namespace EnemyNS.Sound
{
    [RequireComponent(typeof(EnemyMovement))]
    public class EnemyFootsteps : AudioSetup
    {
        [SerializeField] SurfaceEnemyManager surfaceEnemyManager;
        [Tooltip("This is used to determine what distance has to be traveled in order to play the footstep sound.")]
        [SerializeField] float distanceBetweenSteps = 1.8f;
        [Tooltip("To know how much the character moved.")]
        [SerializeField] NavMeshAgent Agent;
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
        bool previouslyGrounded;
        bool isGrounded = true;
        private void Start()
        {
            if (!Agent)
                Agent = GetComponent<EnemyMovement>().Agent;
            if (groundLayers.value == 0)
                groundLayers = 1;
            thisTransform = transform;
            string errorMessage = "";
            if (!audioSource) errorMessage = "No audio source assigned in the inspector, footsteps cannot be played";
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
            if (Agent.velocity.magnitude > 0.01f)
            {
                if (isGrounded)
                {
                    // Advance the step cycle only if the character is grounded.
                    AdvanceStepCycle(Agent.speed * Time.deltaTime);
                }
            }
        }
        public void TryPlayFootstep()
        {
            if (isGrounded)
                PlayFootstep();
        }
        public void PlayLandSound() =>
           audioSource.PlayOneShot(surfaceEnemyManager.GetLandsound(currentGroundInfo.collider, currentGroundInfo.point), landVolume * GetVolumeOfType(audioKind));
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
            AudioClip randomFootstep = surfaceEnemyManager.GetFootstep(currentGroundInfo.collider, currentGroundInfo.point);
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
            if (!previouslyGrounded && isGrounded && GetComponent<EnemyMovement>().State != EnemyState.Dead)
                PlayLandSound();
            // print(isGrounded);
        }
    }
}

