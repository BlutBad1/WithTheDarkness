using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyNS.Skills
{
    public class SkillScriptableObject : ScriptableObject
    {
        [SerializeField, FormerlySerializedAs("Cooldown")]
        protected float cooldown = 10f;
        [SerializeField, FormerlySerializedAs("Damage")]
        protected int damage = 5;

        protected float timeFromLastUse = 0f;
        protected bool isSkillActive;

        public bool IsSkillActive { get => isSkillActive; }

        private void OnEnable()
        {
            timeFromLastUse = 0f;
            isSkillActive = false;
        }
        public virtual void UseSkill(EnemySkillInfo enemySkillInfo) =>
            isSkillActive = true;
        public virtual bool CanUseSkill(EnemySkillInfo enemySkillInfo) =>
             !enemySkillInfo.Damageable.IsDead&& !isSkillActive && timeFromLastUse + cooldown < Time.time;
    }
}