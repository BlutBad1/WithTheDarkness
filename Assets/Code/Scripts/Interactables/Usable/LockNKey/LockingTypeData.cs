using ScriptableObjectNS.Singleton;
using System.Collections.Generic;
using UnityEngine;
namespace ScriptableObjectNS.Locking
{
    [CreateAssetMenu(fileName = "LockingTypeData", menuName = "ScriptableObject/Locking/LockingTypeData")]
    public class LockingTypeData : SingletonScriptableObject<LockingTypeData>
    {
        public List<string> LockingTypes;
    }
}
