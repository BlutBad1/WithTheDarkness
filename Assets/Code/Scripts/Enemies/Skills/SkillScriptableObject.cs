using EnemyNS.Base;
using UnityEngine;
namespace EnemyNS.Skills
{
    public class SkillScriptableObject : ScriptableObject
    {
        public float Cooldown = 10f;
        public int Damage = 5;
        [HideInInspector]
        public bool IsActivating;
        protected float UseTime = 0f;
        private void OnEnable()
        {
            UseTime = 0f;
            IsActivating = false;
        }
        public virtual void UseSkill(Enemy enemy, GameObject target) =>
            IsActivating = true;
        public virtual bool CanUseSkill(Enemy enemy, GameObject target) =>
             !enemy.IsDead;
    }
}