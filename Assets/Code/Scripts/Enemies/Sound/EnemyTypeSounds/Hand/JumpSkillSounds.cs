using EnemySoundNS;
using UnityEngine;

namespace EnemySoundNS
{
    public class JumpSkillSounds : MonoBehaviour
    {
        EnemyFootsteps enemyFootsteps;
        private void Start() =>
            enemyFootsteps = GetComponentInParent<EnemyFootsteps>();
        public void PlayLandSound() =>
            enemyFootsteps.PlayLandSound();
    }
}