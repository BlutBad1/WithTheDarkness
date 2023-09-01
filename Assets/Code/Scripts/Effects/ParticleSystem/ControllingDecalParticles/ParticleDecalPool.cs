using UnityEngine;

namespace ControllingParticlesNS
{


    public class ParticleDecalPool : MonoBehaviour
    {
        private int particleDecalIndex;//ітератор для прохождення по масивам 
        public int maxDecals = 100;//кількість об'єктів в басейні 
        public float decalsMinSize = 100;//мінімальний розмір об'єкта
        public float decalsMaxSize = 100;//максимальний розмір об'єкта
        public ParticleDecalData[] particleData;//масив інформації, позиція, поворот і т.д.
        private ParticleSystem.Particle[] particles;//масив самих ефектів
        private ParticleSystem decalParticalSystem;//система, яка запускає ефект 
        void Start()
        {
            decalParticalSystem = GetComponent<ParticleSystem>();
            particles = new ParticleSystem.Particle[maxDecals];
            particleData = new ParticleDecalData[maxDecals];
            for (int i = 0; i < maxDecals; i++)
            {
                particleData[i] = new ParticleDecalData();
            }
        }
        public void ParticleHit(ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient)
        {
            SetParticleData(particleCollisionEvent, colorGradient);
            DisplayParticles();
        }
        void SetParticleData(ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient)
        {
            if (particleDecalIndex >= maxDecals)
            {
                particleDecalIndex = 0;
            }
            particleData[particleDecalIndex].Position = particleCollisionEvent.intersection;
            Vector3 particleRotationEuler = Quaternion.LookRotation(particleCollisionEvent.normal).eulerAngles;
            particleRotationEuler.z = Random.Range(0, 360);
            particleData[particleDecalIndex].Rotation = particleRotationEuler;
            particleData[particleDecalIndex].Size = Random.Range(decalsMinSize, decalsMaxSize);
            particleData[particleDecalIndex].Color = colorGradient.Evaluate(Random.Range(0f, 1f));


            particleDecalIndex++;

        }

        void DisplayParticles()
        {
            for (int i = 0; i < particleData.Length; i++)
            {
                particles[i].position = particleData[i].Position;
                particles[i].rotation3D = particleData[i].Rotation;
                particles[i].size = particleData[i].Size;
                particles[i].color = particleData[i].Color;
            }
            decalParticalSystem.SetParticles(particles, particles.Length);

        }

    }
}
