using DamageableNS;
using EnemyNS.Attack;
using EnemyNS.Base;
using System;
using System.Collections;
using UnityEngine;

namespace EnemyNS.Skills
{
    [Serializable]
    public class EnemySkillInfo
    {
        public Damageable Damageable;
        public EnemyMovement EnemyMovement;
        public Animator Animator;
        public EnemyAttack EnemyAttack;
        [HideInInspector]
        public GameObject PursuedTarget;
        [HideInInspector]
        public Coroutine SkillCoroutine;
    }
    public class EnemyUseSkills : MonoBehaviour
    {
        [SerializeField]
        private StateHandler stateHandler;
        [SerializeField]
        private Enemy enemy;
        [SerializeField]
        private EnemySkillInfo enemySkillInfo;

        private SkillScriptableObject[] skills;
        private Coroutine skillCoroutine;
        private Coroutine waitSkillCoroutine;
        private bool isUsingSkill = false;

        public SkillScriptableObject[] Skills { get => skills; set => skills = value; }

        private void Start()
        {
            enemySkillInfo.SkillCoroutine = skillCoroutine;
        }
        private void Update()
        {
            TryUseSkills();
        }
        public void StopSkills()
        {
            ClearSkillCoroutine();
        }
        protected void ClearSkillCoroutine()
        {
            if (skillCoroutine != null)
                StopCoroutine(skillCoroutine);
            if (waitSkillCoroutine != null)
                StopCoroutine(waitSkillCoroutine);
        }
        private void TryUseSkills()
        {
            if (skills != null && !isUsingSkill)
                UseSkill(skills[UnityEngine.Random.Range(0, skills.Length)]);
        }
        private void UseSkill(SkillScriptableObject skillScriptableObject)
        {
            GameObject pursuedTarget = stateHandler.PursuedTarget;
            if (pursuedTarget)
            {
                enemySkillInfo.PursuedTarget = pursuedTarget;
                ClearSkillCoroutine();
                if (skillScriptableObject.CanUseSkill(enemySkillInfo))
                {
                    isUsingSkill = true;
                    skillScriptableObject.UseSkill(enemySkillInfo);
                    waitSkillCoroutine = StartCoroutine(WaitUntilSkillEnd(skillScriptableObject));
                }
            }
        }
        private IEnumerator WaitUntilSkillEnd(SkillScriptableObject skill)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
            while (skill.IsSkillActive)
                yield return waitForSeconds;
            isUsingSkill = false;
            waitSkillCoroutine = null;
        }
    }
}
