using UnityEngine;

public class SkillScriptableObject : ScriptableObject
{
    public float Cooldown = 10f;
    public int Damage = 5;
    public bool IsActivating;
    protected float UseTime;


    public virtual void UseSkill(Enemy enemy, GameObject player)
    {
        IsActivating = true;
    }
    public virtual bool CanUseSkill(Enemy enemy, GameObject player)
    {
        return true;
    }
}
