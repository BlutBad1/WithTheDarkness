using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ControllingParticlesNS
{


public class ParticleLauncher : MonoBehaviour
{
        public ParticleSystem splatterParticles;
        public Gradient particleColodGradient; 
        private List<ParticleCollisionEvent> collisionEvents;
        public ParticleDecalPool[] splatDecalPool;
       //public InputManager inputManager;
        // Start is called before the first frame update
        void Start()
        {
            collisionEvents=new List<ParticleCollisionEvent>();
        }
        private void OnParticleCollision(GameObject other)
        {
            ParticlePhysicsExtensions.GetCollisionEvents(splatterParticles, other, collisionEvents);
            for (int i = 0; i < collisionEvents.Count; i++)
            {

                splatDecalPool[Random.Range(0,splatDecalPool.Length)].ParticleHit(collisionEvents[i], particleColodGradient);
               
               // EmitAtLocation(collisionEvents[i]);
            }
          
        }
        void EmitAtLocation(ParticleCollisionEvent particleCollision)
        {
          //  splatDecalPool.transform.position = particleCollision.intersection;
         //   splatDecalPool.transform.rotation = Quaternion.LookRotation(particleCollision.normal);
          
        }
        // Update is called once per frame
        //void Update()
        //{
        //    if (inputManager.OnFoot.Firing.triggered)
        //    {
        //        splatterParticles.Emit(1);
        //    }

        //}
    }
}