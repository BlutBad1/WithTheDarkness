using DamageableNS;
using EntityNS.Attack;
using EntityNS.Base;
using System;
using System.Collections;
using UnityEngine;

namespace EntityNS.Skills
{
    [Serializable]
    public class EntitySkillInfo
    {
        public Damageable Damageable;
        public EntityMovement EntityMovement;
        public Animator Animator;
        public EntityAttack EntityAttack;
        [HideInInspector]
        public GameObject PursuedTarget;
        [HideInInspector]
        public Coroutine SkillCoroutine;
    }
    public class EntityUseSkills : MonoBehaviour
    {
        [SerializeField]
        private StateHandler stateHandler;
        [SerializeField]
        private EntityBehaviour entityBehaviour;
        [SerializeField]
        private EntitySkillInfo entitySkillInfo;

        private SkillScriptableObject[] skills;
        private Coroutine skillCoroutine;
        private Coroutine waitSkillCoroutine;
        private bool isUsingSkill = false;

        public SkillScriptableObject[] Skills { get => skills; set => skills = value; }

        private void Start()
        {
            entitySkillInfo.SkillCoroutine = skillCoroutine;
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
                entitySkillInfo.PursuedTarget = pursuedTarget;
                ClearSkillCoroutine();
                if (skillScriptableObject.CanUseSkill(entitySkillInfo))
                {
                    isUsingSkill = true;
                    skillScriptableObject.UseSkill(entitySkillInfo);
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
