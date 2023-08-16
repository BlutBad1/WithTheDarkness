using PlayerScriptsNS;
using System.Collections;
using UnityEngine;

namespace GameObjectsControllingNS
{
    public class SpawnPlayerOnPosition : MonoBehaviour
    {
        [SerializeField]
        public Transform Transform;
        private void Awake()
        {
            StartCoroutine(SpawnPlayer());
        }
        IEnumerator SpawnPlayer()
        {
            GameObject Player = GameObject.Find(MyConstants.CommonConstants.PLAYER);
            while (Player == null)
            {
                Player = GameObject.Find(MyConstants.CommonConstants.PLAYER);
                yield return null;
            }
            Player.GetComponent<InputManager>().IsMovingEnable = false;
            yield return new WaitForSeconds(0.05f);
            if (!Transform)
                Transform = transform;
            Player.transform.position = Transform.position;
            Player.transform.rotation = Transform.rotation;
            yield return new WaitForSeconds(0.05f);
            Player.GetComponent<InputManager>().IsMovingEnable = true;
            Destroy(this);
        }
    }
}