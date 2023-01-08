using UnityEngine;

public class SkillScriptableObject : ScriptableObject
{
    public float Cooldown = 10f;
    public int Damage = 5;
    public bool IsActivating;
    protected float UseTime;


    //public virtual void UseSkill(Enemy enemy , Player player )
    //{
    //    IsActivating = true;
    //}
    //public virtual bool CanUseSkill(Enemy enemy, Player player)
    //{
    //    return true;
    //}
}
