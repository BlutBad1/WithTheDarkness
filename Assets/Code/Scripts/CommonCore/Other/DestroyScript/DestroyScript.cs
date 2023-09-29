using System.Collections;
using UnityEngine;

namespace DestroyScriptNS
{
    public class DestroyScript : MonoBehaviour
    {
        public void DestroyThisScript(MonoBehaviour script) =>
            Destroy(script);
    }
}
