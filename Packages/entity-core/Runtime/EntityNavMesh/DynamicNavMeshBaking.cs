using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace EntityNS.Navigation
{
	public class DynamicNavMeshBaking : MonoBehaviour
	{
		public static DynamicNavMeshBaking Instance { get; private set; }

		[SerializeField]
		private NavMeshSurface[] surfaces;
		[SerializeField]
		private GameObject player;
		[SerializeField]
		private float updateRate = 0.1f;
		[SerializeField]
		private float movementThreshold = 3f;
		[SerializeField]
		private Vector3 navMeshSize = new Vector3(20, 20, 20);

		private Dictionary<NavMeshSurface, NavMeshData> navMeshDatas = new Dictionary<NavMeshSurface, NavMeshData>();
		private Dictionary<NavMeshSurface, Coroutine> deleteNavMesh = new Dictionary<NavMeshSurface, Coroutine>();
		private List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();

		private void Awake()
		{
			for (int i = 0; i < surfaces.Length; i++)
			{
				navMeshDatas.Add(surfaces[i], new NavMeshData());
				NavMesh.AddNavMeshData(navMeshDatas[surfaces[i]]);
			}
			if (Instance == null)
				Instance = this;
			else
				Destroy(this);
		}
		public bool CheckIfInNavMeshRange(GameObject gameObject)
		{
			Vector3 surfacePosition = player.transform.position;
			Bounds navMeshBounds = new Bounds(surfacePosition, navMeshSize);
			return navMeshBounds.Contains(gameObject.transform.position);
		}
		public void CreateNavMeshByAgentId(int agentId, bool async = true)
		{
			Vector3 surfacePosition = player.transform.position;
			Bounds navMeshBounds = new Bounds(surfacePosition, navMeshSize);
			NavMeshSurface navMeshSurface = surfaces.FirstOrDefault(x => x.agentTypeID == agentId) ?? null;
			if (navMeshSurface != null)
			{
				BuildNavMeshSurface(async, navMeshBounds, navMeshSurface);
				if (deleteNavMesh.ContainsKey(navMeshSurface) && deleteNavMesh[navMeshSurface] != null)
					StopCoroutine(deleteNavMesh[navMeshSurface]);
				deleteNavMesh[navMeshSurface] = StartCoroutine(DeleteNavMeshIfPlayerFar(navMeshSurface, surfacePosition));
			}
		}
		private void BuildNavMeshes(bool async)
		{
			Bounds navMeshBounds = new Bounds(player.transform.position, navMeshSize);
			for (int index = 0; index < surfaces.Length; index++)
				BuildNavMeshSurface(async, navMeshBounds, surfaces[index]);
		}
		private IEnumerator DeleteNavMeshIfPlayerFar(NavMeshSurface navMeshSurface, Vector3 surfacePosition)
		{
			Bounds bounds = new Bounds(player.transform.position, Vector3.zero);
			while (Vector3.Distance(surfacePosition, player.transform.position) < movementThreshold)
				yield return updateRate;
			NavMeshBuilder.UpdateNavMeshDataAsync(navMeshDatas[navMeshSurface], navMeshSurface.GetBuildSettings(), sources, bounds);
		}
		private void BuildNavMeshSurface(bool async, Bounds navMeshBounds, NavMeshSurface navMeshSurface)
		{
			List<NavMeshBuildMarkup> markups = new List<NavMeshBuildMarkup>();
			List<NavMeshModifier> modifiers;
			if (navMeshSurface.collectObjects == CollectObjects.Children)
				modifiers = new List<NavMeshModifier>(GetComponentsInChildren<NavMeshModifier>());
			else
				modifiers = NavMeshModifier.activeModifiers;
			for (int i = 0; i < modifiers.Count; i++)
			{
				if (((navMeshSurface.layerMask & (1 << modifiers[i].gameObject.layer)) == 1)
					&& modifiers[i].AffectsAgentType(navMeshSurface.agentTypeID))
				{
					markups.Add(new NavMeshBuildMarkup()
					{
						root = modifiers[i].transform,
						overrideArea = modifiers[i].overrideArea,
						area = modifiers[i].area,
						ignoreFromBuild = modifiers[i].ignoreFromBuild
					});
				}
			}
			if (navMeshSurface.collectObjects == CollectObjects.Children)
				NavMeshBuilder.CollectSources(transform, navMeshSurface.layerMask, navMeshSurface.useGeometry, navMeshSurface.defaultArea, markups, sources);
			else
				NavMeshBuilder.CollectSources(navMeshBounds, navMeshSurface.layerMask, navMeshSurface.useGeometry, navMeshSurface.defaultArea, markups, sources);
			//sources.RemoveAll(RemoveNavMeshAgentPredicate);
			if (async)
				NavMeshBuilder.UpdateNavMeshDataAsync(navMeshDatas[navMeshSurface], navMeshSurface.GetBuildSettings(), sources, navMeshBounds);
			else
				NavMeshBuilder.UpdateNavMeshData(navMeshDatas[navMeshSurface], navMeshSurface.GetBuildSettings(), sources, navMeshBounds);
		}
	}
}
