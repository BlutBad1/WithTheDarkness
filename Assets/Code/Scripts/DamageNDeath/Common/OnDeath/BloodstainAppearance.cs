using UnityEngine;

namespace OnDeath
{
    public class BloodstainAppearance : MonoBehaviour
    {
        public Damageable Damageable;
        public GameObject BloodstainPrefab;
        public Vector3 BloodstainSize;
        [Tooltip("The location from which a ray will be fired to detect the surface.")]
        public Transform OriginOfRay;
        public Transform ParentOfBloodstain;
        [SerializeField]
        public LayerMask WhatIsRayCastIgnore;
        public float MaxDistance;
        private GameObject initializedBloodstain;
        static ObjectPool objectPool;
        private void Start()
        {
            if (!Damageable)
                Damageable = GetComponent<Damageable>();
            if (Damageable)
                Damageable.OnDeath += InitializeBloodstain;
        }
        private void OnDisable()
        {
            if (Damageable)
                Damageable.OnDeath -= InitializeBloodstain;
        }
        public void InitializeBloodstain()
        {
            if (!OriginOfRay)
                OriginOfRay = transform;
            if (Physics.Raycast(OriginOfRay.position, -Vector3.up, out RaycastHit hitInfo, MaxDistance, ~WhatIsRayCastIgnore))
            {
                 if (objectPool == null)
                    objectPool = ObjectPool.CreateInstance(BloodstainPrefab, 20);
                initializedBloodstain = objectPool.GetObject();
                initializedBloodstain.SetActive(true);
                initializedBloodstain.transform.parent = null;
                initializedBloodstain.transform.position = hitInfo.point + (hitInfo.normal * 0.01f);
                initializedBloodstain.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
                initializedBloodstain.transform.localScale = BloodstainSize;
                if (!ParentOfBloodstain)
                    ParentOfBloodstain = hitInfo.transform;
                initializedBloodstain.transform.parent = ParentOfBloodstain;
                if (!initializedBloodstain.TryGetComponent(out ParticleSystem particleSystem))
                {
                    if (initializedBloodstain.GetComponentInChildren<ParticleSystem>())
                        initializedBloodstain.GetComponentInChildren<ParticleSystem>().Play();
                }
                else
                    particleSystem.Play();
            }
        }
    }
}