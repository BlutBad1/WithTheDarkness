using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyFootsteps : MonoBehaviour
{

	[SerializeField] SurfaceEnemyManager surfaceEnemyManager;
	[Tooltip("This is used to determine what distance has to be traveled in order to play the footstep sound.")]
	[SerializeField] float distanceBetweenSteps = 1.8f;

	[Tooltip("To know how much the character moved.")]

	[SerializeField] NavMeshAgent Agent;
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
	float lastPlayTime;
	bool previouslyGrounded;
	bool isGrounded;


	void Start()
	{
		if (groundLayers.value == 0)
		{
			groundLayers = 1;
		}

		thisTransform = transform;
		string errorMessage = "";

		if (!audioSource) errorMessage = "No audio source assigned in the inspector, footsteps cannot be played";
	
	

		if (errorMessage != "")
		{
			Debug.LogError(errorMessage);
			enabled = false;
		}
	}

	void Update()
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
		{
			PlayFootstep();
		}
	}

	void PlayLandSound()
	{

		audioSource.PlayOneShot(surfaceEnemyManager.GetLandsound(currentGroundInfo.collider, currentGroundInfo.point), landVolume);
	}

	void AdvanceStepCycle(float increment)
	{
		stepCycleProgress += increment;

		if (stepCycleProgress > distanceBetweenSteps)
		{
			
			stepCycleProgress = 0f;

			PlayFootstep();
		}
	}

	void PlayFootstep()
	{

		AudioClip randomFootstep = surfaceEnemyManager.GetFootstep(currentGroundInfo.collider, currentGroundInfo.point);
		float randomVolume = Random.Range(minVolume, maxVolume);

		if (randomFootstep)
		{

			audioSource.PlayOneShot(randomFootstep, randomVolume);
		}
	}

	void OnDrawGizmos()
	{
		if (debugMode)
		{
			Gizmos.DrawWireSphere(transform.position + Vector3.up * groundCheckHeight, groundCheckRadius);
			Gizmos.color = Color.red;
			Gizmos.DrawRay(transform.position + Vector3.up * groundCheckHeight, Vector3.down * (groundCheckDistance + groundCheckRadius));
		}
	}

	void CheckGround()
	{

		previouslyGrounded = isGrounded;
		Ray ray = new Ray(thisTransform.position + Vector3.up * groundCheckHeight, Vector3.down);

		if (Physics.SphereCast(ray, groundCheckRadius, out currentGroundInfo, groundCheckDistance, groundLayers, QueryTriggerInteraction.Ignore))
		{
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}

		if (!previouslyGrounded && isGrounded)
		{
			PlayLandSound();
		}
		// print(isGrounded);
	}
}



