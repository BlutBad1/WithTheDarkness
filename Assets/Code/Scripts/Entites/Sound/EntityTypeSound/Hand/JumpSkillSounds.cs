using EntityNS.Sound;
using UnityEngine;

namespace EntityNS.Sound
{
    public class JumpSkillSounds : MonoBehaviour
    {
        EntityFootsteps enemyFootsteps;
        private void Start() =>
            enemyFootsteps = GetComponentInParent<EntityFootsteps>();
        public void PlayLandSound() =>
            enemyFootsteps.PlayLandSound();
    }
}