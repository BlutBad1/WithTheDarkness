using PlayerScriptsNS;
using UnityEngine;
namespace LightNS
{

    public class LampAndHandAnimations : MonoBehaviour
    {
        [SerializeField] PlayerMotor characterController;
        [SerializeField]
        private GameObject left_hand;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float speed = characterController.currentVelocity.magnitude;

            left_hand.GetComponent<Animator>().SetFloat("speed", speed);
        }
    }
}