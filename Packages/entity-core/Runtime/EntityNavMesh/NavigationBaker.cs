using System.Collections;
using UnityEngine;
using UnityEngine.AI;
namespace EntityNS.Navigation
{
    public class NavigationBaker : MonoBehaviour
    {
        [SerializeField]
        private NavMeshSurface[] surfaces;

        private void Awake() =>
            StartCoroutine(BakeSurfaces());
        private IEnumerator BakeSurfaces()
        {
            if (surfaces == null || surfaces?.Length <= 0)
                surfaces = GetComponents<NavMeshSurface>();
            if (surfaces?.Length <= 0)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Surfaces are not defined!");
#endif
            }
            else
            {
                for (int i = 0; i < surfaces?.Length; i++)
                    surfaces[i].BuildNavMesh();
            }
            yield return null;
        }
    }
}