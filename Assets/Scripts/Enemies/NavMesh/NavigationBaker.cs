using System.Collections;
using UnityEngine;
using UnityEngine.AI;
namespace EnemyNavigationNS
{
    public class NavigationBaker : MonoBehaviour
    {
        public NavMeshSurface[] surfaces;
        private void Awake() =>
            StartCoroutine(BakeSurfaces());
        IEnumerator BakeSurfaces()
        {
            //while (playerInputManager.IsMovingEnable)
            //{
            //    yield return null;
            //}
            if (surfaces.Length <= 0)
                surfaces = GetComponents<NavMeshSurface>();
#if UNITY_EDITOR
            if (surfaces.Length <= 0)
                Debug.LogWarning("Surfaces are not defined!");
#endif
            else
            {
                for (int i = 0; i < surfaces.Length; i++)
                    surfaces[i].BuildNavMesh();
            }
            yield return null;
        }
    }
}