using UnityEngine;
using UnityEngine.Serialization;

namespace EntityNS.Skills
{
    public class SkillScriptableObject : ScriptableObject
    {
        [SerializeField]
        protected float cooldown = 10f;
        [SerializeField]
        protected int damage = 5;

        protected float timeFromLastUse = 0f;
        protected bool isSkillActive;

        public bool IsSkillActive { get => isSkillActive; }

        private void OnEnable()
        {
            timeFromLastUse = 0f;
            isSkillActive = false;
        }
        public virtual void UseSkill(EntitySkillInfo enemySkillInfo) =>
            isSkillActive = true;
        public virtual bool CanUseSkill(EntitySkillInfo enemySkillInfo) =>
             !enemySkillInfo.Damageable.IsDead&& !isSkillActive && timeFromLastUse + cooldown < Time.time;
    }
}