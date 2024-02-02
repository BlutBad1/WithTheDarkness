using DamageableNS;
using InteractableNS.Common;
using MyConstants.CreatureConstants.EnemyConstants;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyNS.Death
{
    public class GramophoneOnDeath : EnemyDeadEvent
    {
        [SerializeField]
        private Renderer[] gramaphonePartRendereres;
        [SerializeField]
        private float innerGameoObjectsMass = 20f;
        [SerializeField]
        private float innerGameoObjectsPushForce = 50f;
        [SerializeField, FormerlySerializedAs("GramophoneInnerGameObjects")]
        private GameObject[] gramophoneInnerGameObjects;

        private List<Rigidbody> rigidbodies = new List<Rigidbody>();

        protected override void OnDisable()
        {
            base.OnDisable();
            gameObject.SetActive(false);
        }
        protected override void OnDeadEvent()
        {
            base.OnDeadEvent();
            animator.enabled = true;
            animator.SetTrigger(GramophoneConstants.STOP_PLAYING_TRIGGER);
            ItemImpact itemImpact;
            Rigidbody rigidbody;
            foreach (var item in gramophoneInnerGameObjects)
            {
                item.AddComponent<Damageable>();
                rigidbody = item.AddComponent<Rigidbody>();
                rigidbody.mass = innerGameoObjectsMass;
                itemImpact = item.AddComponent<ItemImpact>();
                itemImpact.PushForce = innerGameoObjectsPushForce;
                rigidbodies.Add(rigidbody);
            }
            StartCoroutine(DisableGramaphoneParts());
        }
        private IEnumerator DisableGramaphoneParts()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
            while (gramaphonePartRendereres.Where(x => x.gameObject.activeInHierarchy).Count() != 0)
            {
                foreach (var part in gramaphonePartRendereres)
                {
                    if (!UtilitiesNS.RendererNS.CheckRenderVisibility.IsRendererVisibleWithinCameraBounds(part, Camera.main))
                    {
                        part.gameObject.SetActive(false);
                        rigidbodies.ForEach(x => x.WakeUp());
                    }
                }
                yield return waitForSeconds;
            }
        }
    }
}
