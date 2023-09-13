using EnemyNS.Base;
using System.Collections;
using UnityEngine;

namespace EnemyNS.Type.Gramophone
{
    public class GramophoneMovement : EnemyMovement
    {
        private void OnTriggerEnter(Collider other)
        {
            HandleGainCreatureInSight(other.gameObject);
        }
        //private void OnTriggerExit(Collider other)
        //{
        //    if (Opponents.ContainsKey(other.gameObject))
        //        Opponents.Remove(other.gameObject);
        //    if (other.gameObject == PursuedTarget)
        //    {
        //        PursuedTarget = null;
        //        State = DefaultState;
        //    }
        //}
        protected override IEnumerator DoFollowTarget()
        {
            Animator.SetTrigger(MyConstants.CreatureConstants.EnemyConstants.GramophoneConstants.PLAY_TRIGGER);
            return base.DoFollowTarget();
        }

        protected override IEnumerator DoIdleMotion()
        {
            yield return null;
        }
    }
}