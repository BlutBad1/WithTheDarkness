using CreatureConstantsNS;
using CreatureNS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EntityNS.Base
{
    public class EntityCreature : Creature, ISerializationCallbackReceiver
    {
        [SerializeField, ListToMultiplePopup(typeof(Creature), "creatureNames")]
        protected int opponentsMask;
        [SerializeField]
        private NavMeshAgent agent;

        private bool isOriginalSpeedSet = false;
        private float originalSpeed;
        private List<string> opponentList;

        public string CreatureType { get => GetCreatureType(); }
        private float OrigininalSpeed
        {
            get
            {
                if (!isOriginalSpeedSet)
                {
                    isOriginalSpeedSet = true;
                    originalSpeed = agent.speed;
                }
                return originalSpeed;
            }
        }
        private void Start()
        {
            opponentList = GetOpponentsList();
        }
        public override void BlockMovement()
        {
            if (agent)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
        }
        public override void UnblockMovement()
        {
            if (agent)
                agent.isStopped = false;
        }
        public CreatureRelation DefineRelation(ICreature creature)
        {
            if (opponentList.Contains(creature.GetCreatureType()))
                return CreatureRelation.Enemy;
            return CreatureRelation.Friend;
        }
        public override void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            BlockMovement();
            transform.position = position;
            transform.rotation = rotation;
            UnblockMovement();
        }
        protected override void SetCurrentSpeed() =>
            agent.speed = OrigininalSpeed * CurrentSpeedCoefficient;
        protected List<string> GetOpponentsList()
        {
            List<string> list = new List<string>();
            if (CreatureType == CreatureConstants.ALONE_CREATURE_TYPE)
                list = CreatureNames;
            else
            {
                for (int i = 0; i < CreatureNames.Count; i++)
                {
                    if ((opponentsMask & (1 << i)) != 0)
                        list.Add(CreatureNames[i]);
                }
            }
            return list;
        }
    }
}