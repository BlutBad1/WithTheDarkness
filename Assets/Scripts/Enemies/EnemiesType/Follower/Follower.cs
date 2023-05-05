using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
namespace EnemyFollowerNS
{
    public class Follower : MonoBehaviour
    {
        public GameObject[] FollowersObjects;
        public float FollowingDistance = 10f;
        public float StoppingDistance = 1.5f;
        public float FollowSpeed = 1f;
        public float UpdatingSpeed = 0.1f;
        public GameObject Player;
        Coroutine followCoroutine;
        public bool alwaysMultithreading = false;
        /*If count of locations greater that this number, then multithreading would be enable automatically.
         -1 to disable.
         */
        public int enableAutomaticallyIfGreater = 75;
        TransformAccessArray m_accessArray;
        List<Transform> gameObjectsTransforms;
        private void Awake()
        {
            gameObjectsTransforms = new List<Transform>();
            for (int i = 0; i < FollowersObjects?.Length; i++)
                gameObjectsTransforms.Add(FollowersObjects[i].transform);
        }
        public void AddGameObject(GameObject gameObject)
        {
            if (FollowersObjects == null)
                FollowersObjects = new GameObject[0];
            Array.Resize(ref FollowersObjects, FollowersObjects.Length + 1);
            FollowersObjects[^1] = gameObject;
            gameObjectsTransforms.Add(gameObject.transform);
        }
        private void Start()
        {
            if (!Player)
                Player = GameObject.Find(MyConstants.CommonConstants.PLAYER);
#if UNITY_EDITOR
            if (!Player)
                UnityEngine.Debug.LogWarning("Player is not found");
#endif
        }
        void Update()
        {
            if (Vector3.Distance(transform.position, Player.transform.position) <= FollowingDistance)
            {
                if (followCoroutine == null)
                    followCoroutine = StartCoroutine(StartFollow());
            }
        }
        IEnumerator StartFollow()
        {
            if ((FollowersObjects?.Length > enableAutomaticallyIfGreater && enableAutomaticallyIfGreater != -1) || alwaysMultithreading)
            {
                m_accessArray = new TransformAccessArray(gameObjectsTransforms.ToArray());
                var job = new FollowJob()
                {
                    _playerPosition = Player.transform.position,
                    _followSpeed = FollowSpeed,
                    _followingDistance = FollowingDistance,
                    _stoppingDistance = StoppingDistance,
                    _deltaTime = Time.deltaTime
                };

                JobHandle jobHandle = job.Schedule(m_accessArray);
                jobHandle.Complete();
                m_accessArray.Dispose();
            }
            else
            {
                for (int i = 0; i < FollowersObjects?.Length; i++)
                    if (Vector3.Distance(FollowersObjects[i].transform.position, Player.transform.position) <= FollowingDistance && Vector3.Distance(FollowersObjects[i].transform.position, Player.transform.position) > StoppingDistance)
                        FollowersObjects[i].transform.position = Vector3.Lerp(FollowersObjects[i].transform.position, Player.transform.position, FollowSpeed * Time.deltaTime);
            }
            yield return new WaitForSeconds(UpdatingSpeed);
            followCoroutine = null;

        }

        public struct FollowJob : IJobParallelForTransform
        {
            public Vector3 _playerPosition;
            public float _followSpeed;
            public float _followingDistance;
            public float _stoppingDistance;
            public float _deltaTime;
            public void Execute(int index, TransformAccess transform)
            {
                if (Vector3.Distance(transform.position, _playerPosition) <= _followingDistance && Vector3.Distance(transform.position, _playerPosition) > _stoppingDistance)
                    transform.position = Vector3.Lerp(transform.position, _playerPosition, _followSpeed * _deltaTime);
            }
        }
    }
}