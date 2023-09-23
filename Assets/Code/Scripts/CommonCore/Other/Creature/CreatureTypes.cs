using ScriptableObjectNS.Singleton;
using System.Collections.Generic;
namespace ScriptableObjectNS.Creature
{
    //[CreateAssetMenu(fileName = "Creature Types", menuName = "ScriptableObject/CreatureTypes")]
    public class CreatureTypes : SingletonScriptableObject<CreatureTypes>
    {
        //private void Awake()
        //{
        //    if (Instance == null)
        //        Instance = this;
        //    else if (Instance != this)
        //        DestroyImmediate(this);
        //}
        public List<string> Names;
    }
}