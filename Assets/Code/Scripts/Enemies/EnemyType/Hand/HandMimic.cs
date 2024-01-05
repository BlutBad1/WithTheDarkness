using DG.Tweening;
using PlayerScriptsNS;
using UnityEngine;
namespace EnemyNS.Type.Hand
{
    public class HandMimic : MonoBehaviour
    {
        public float DOMoveDuration = 10f;
        private static PlayerCreature player;
        private void Start()
        {
            if (!player)
                player = FindAnyObjectByType<PlayerCreature>();
        }
        private void Update()
        {
            transform.LookAt(player.gameObject.transform);
            Vector3 newPosition = new Vector3(player.gameObject.transform.position.x, transform.position.y, player.gameObject.transform.position.z);
            transform.DOMove(newPosition, DOMoveDuration);
        }
    }
}
