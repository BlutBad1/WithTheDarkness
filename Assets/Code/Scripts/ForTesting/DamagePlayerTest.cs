using DamageableNS;
using UnityEngine;
namespace ForTestingNS
{
    public class DamagePlayerTest : MonoBehaviour
    {
        [SerializeField]
        public GameObject player;
        void Start()
        {
            if (!player)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Player is not found!");
#endif
            }
        }
        public void AttackPlayer(int damage) =>
            player.GetComponent<Damageable>().TakeDamage(new TakeDamageData(player.GetComponent<Damageable>(),damage, 0, null, null));
    }
}