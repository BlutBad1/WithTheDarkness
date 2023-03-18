using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyBaseNS;
using System;

namespace EnemyOnTakeDameNS
{ 
public class TakeDamageSound : MonoBehaviour
{
       
        private Enemy enemy;
        public Sound[] Sounds;
        public AudioSource AudioSource;


        void Awake()
        {

            foreach (Sound s in Sounds)
            {
                s.source = AudioSource;

            }
        }

        void Start()
        {
            enemy = GetComponent<Enemy>();
           
            enemy.OnTakeDamage += OnTakeDamage;
        }
        public void PlayRandomHitSound()
        {
            
            Sound s = Sounds[UnityEngine.Random.Range(0, Sounds.Length)];

            s.source.clip = s.clip;
            s.source.volume = s.volume + UnityEngine.Random.Range(-0.2f, 0.2f);
            s.source.pitch = s.pitch + UnityEngine.Random.Range(-0.1f, 0.1f); ;
            s.source.loop = false;
            s.source.playOnAwake = false;

            if (s == null)
            {
                Debug.Log($"Sound is not found!");
                return;
            }

            if (!s.source.isPlaying)
                s.source.Play();

        }
        private void OnTakeDamage(float force, Vector3 hit)
        {
            PlayRandomHitSound();
        }
    }
}
