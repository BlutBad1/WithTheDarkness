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
                player = GameObject.Find(MyConstants.CommonConstants.PLAYER);
            if (!player)
#if UNITY_EDITOR
                Debug.LogWarning("Player is not found!");
#endif
        }
        public void AttackPlayer(int damage) =>
            player.GetComponent<Damageable>().TakeDamage(damage, 0, new Vector3(0, 0, 0));
    }
}