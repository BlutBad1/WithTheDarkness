using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

namespace CommonCore.Colider
{
	public class RagdollEnablerTests
	{
		private GameObject player;
		private GameObject ragdollRoot;
		private GameObject limb;
		private RagdollEnabler enabler;
		[SetUp]
		public void Setup()
		{
			player = GameObject.Instantiate(new GameObject());
			ragdollRoot = GameObject.Instantiate(new GameObject());
			limb = GameObject.Instantiate(new GameObject());
			player.AddComponent<Animator>();
			player.AddComponent<NavMeshAgent>();
			ragdollRoot.AddComponent<Rigidbody>();
			limb.AddComponent<Rigidbody>();
			limb.AddComponent<CharacterJoint>();
			ragdollRoot.transform.parent = player.transform;
			limb.transform.parent = ragdollRoot.transform;
			limb.GetComponent<CharacterJoint>().connectedBody = ragdollRoot.GetComponent<Rigidbody>();
			enabler = player.AddComponent<RagdollEnabler>();

		}

		[UnityTest]
		public IEnumerator DisableAllRigidbodiesTest_Expect_DisabledAllRigidBodies()
		{

			//Act
			enabler.DisableAllRigidbodies();
			//Assert
			yield return new WaitForSeconds(0.1f);
			Assert.IsTrue(ragdollRoot.GetComponent<Rigidbody>().isKinematic);
			Assert.IsFalse(ragdollRoot.GetComponent<Rigidbody>().useGravity);
			Assert.IsFalse(ragdollRoot.GetComponent<Rigidbody>().detectCollisions);
			Assert.IsTrue(limb.GetComponent<Rigidbody>().isKinematic);
			Assert.IsFalse(limb.GetComponent<Rigidbody>().useGravity);
			Assert.IsFalse(limb.GetComponent<Rigidbody>().detectCollisions);
		}
		[UnityTest]
		public IEnumerator EnableRagdollTest_AfterDisableAllRigidbodies_Expect_EnabledRagdolls()
		{

			//Act
			enabler.DisableAllRigidbodies();
			enabler.EnableRagdoll();
			//Assert
			yield return new WaitForSeconds(0.1f);
			Assert.IsFalse(player.GetComponent<Animator>().enabled);
			Assert.IsFalse(player.GetComponent<NavMeshAgent>().enabled);
			Assert.IsFalse(ragdollRoot.GetComponent<Rigidbody>().isKinematic);
			Assert.IsTrue(ragdollRoot.GetComponent<Rigidbody>().useGravity);
			Assert.IsTrue(ragdollRoot.GetComponent<Rigidbody>().detectCollisions);
			Assert.IsFalse(limb.GetComponent<Rigidbody>().isKinematic);
			Assert.IsTrue(limb.GetComponent<CharacterJoint>().enableCollision);
			Assert.IsTrue(limb.GetComponent<Rigidbody>().useGravity);
			Assert.IsTrue(limb.GetComponent<Rigidbody>().detectCollisions);
		}
		[UnityTest]
		public IEnumerator EnableAnimator_AfterDisableAllRigidbodies_Expect_EnabledRagdolls()
		{

			//Act
			enabler.DisableAllRigidbodies();
			enabler.EnableAnimator();
			//Assert
			yield return new WaitForSeconds(0.1f);
			Assert.IsTrue(player.GetComponent<Animator>().enabled);
			Assert.IsTrue(player.GetComponent<NavMeshAgent>().enabled);
			Assert.IsTrue(ragdollRoot.GetComponent<Rigidbody>().isKinematic);
			Assert.IsFalse(ragdollRoot.GetComponent<Rigidbody>().useGravity);
			Assert.IsFalse(ragdollRoot.GetComponent<Rigidbody>().detectCollisions);
			Assert.IsTrue(limb.GetComponent<Rigidbody>().isKinematic);
			Assert.IsFalse(limb.GetComponent<CharacterJoint>().enableCollision);
			Assert.IsFalse(limb.GetComponent<Rigidbody>().useGravity);
			Assert.IsFalse(limb.GetComponent<Rigidbody>().detectCollisions);
		}
	}
}