using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyNS.Navigation
{
	public class NavMeshAgentGenerateNewPath : MonoBehaviour, ISerializationCallbackReceiver
	{
		public static List<string> NavMeshAgentTypes = new List<string>();

		[SerializeField, ListToMultiplePopup(typeof(NavMeshAgentGenerateNewPath), "NavMeshAgentTypes")]
		private int agentType;
		[SerializeField]
		private NavMeshAgent agent;
		[SerializeField]
		private float updateRate = 1f;

		private CancellationTokenSource tokenSource;
		private DynamicNavMeshBaking dynamicNavMeshBaking;

		private int UpdateRate { get => (int)(updateRate * 1000); }

		private void OnEnable()
		{
			tokenSource = new CancellationTokenSource();
		}
		private void OnDisable()
		{
			if (tokenSource != null && !tokenSource.IsCancellationRequested)
			{
				tokenSource.Cancel();
				tokenSource.Dispose();
			}
		}
		private void Start()
		{
			dynamicNavMeshBaking = DynamicNavMeshBaking.Instance;
			CheckAgentNavMesh();
		}
		public void OnAfterDeserialize() { }
		public void OnBeforeSerialize()
		{
			int agentTypeCount = NavMesh.GetSettingsCount();
			for (int i = 0; i < agentTypeCount; i++)
			{
				NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex(i);
				NavMeshAgentTypes.Add(NavMesh.GetSettingsNameFromID(settings.agentTypeID));
			}
		}
		private async void CheckAgentNavMesh()
		{
			while (!tokenSource.IsCancellationRequested)
			{
				if (agent.enabled && !agent.isStopped && agent.isOnOffMeshLink)
					AskToCreatePath();
				await Task.Delay(UpdateRate);
			}
		}
		private void AskToCreatePath()
		{
			if (dynamicNavMeshBaking && dynamicNavMeshBaking.CheckIfInNavMeshRange(gameObject))
			{
				for (int i = 0; i < NavMeshAgentTypes.Count; i++)
				{
					if ((agentType & (1 << i)) != 0)
					{
						int agentId = GetNavMeshAgentID(NavMeshAgentTypes[i]) ?? -1;
						dynamicNavMeshBaking.CreateNavMeshByAgentId(agentId);
					}
				}
			}
		}
		private int? GetNavMeshAgentID(string name)
		{
			for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
			{
				NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex(index: i);
				if (name == NavMesh.GetSettingsNameFromID(agentTypeID: settings.agentTypeID))
					return settings.agentTypeID;
			}
			return null;
		}
	}
}