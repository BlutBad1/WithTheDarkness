using EnemyFootstepsNS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JumpSkillSounds : MonoBehaviour
{
   
    EnemyFootsteps enemyFootsteps;
    private void Start()
    {
    
        enemyFootsteps = GetComponentInParent<EnemyFootsteps>();
    }
    public void PlayLandSound()
    {
        enemyFootsteps.PlayLandSound();
    }
}
