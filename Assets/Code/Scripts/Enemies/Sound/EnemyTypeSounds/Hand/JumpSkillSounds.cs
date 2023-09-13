using EnemyNS.Sound;
using UnityEngine;

namespace EnemyNS.Sound
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