using LightNS;
using UnityEngine;
namespace PortalNS
{
    public class PortalLight : MonoBehaviour
    {
        public Portal ConnectedPortal;
        public Light SpotLight;
        public float LerpSpeed = 100;
        private Vector3 originalSpotLightLocalPosition;
        private PlayerLampSpotLight playerLampSpotLight;
        private void Awake()
        {
            originalSpotLightLocalPosition = SpotLight.transform.localPosition;
            playerLampSpotLight = UnityEngine.Object.FindObjectsOfType<PlayerLampSpotLight>()[0];
            SpotLight.intensity = playerLampSpotLight.SpotLight.intensity;
        }
        private void Update()
        {
            if (!ConnectedPortal.IsPortalVisible())
                return;
            transform.rotation = Quaternion.Lerp(transform.rotation, playerLampSpotLight.SpotLightPivot.transform.rotation, Time.deltaTime * LerpSpeed);
            SpotLight.range = playerLampSpotLight.SpotLight.range;
            SpotLight.spotAngle = playerLampSpotLight.SpotLight.spotAngle;
            // if (transform.eulerAngles.x >= 45f && transform.eulerAngles.z <= 180f)
            SpotLight.transform.localPosition = Vector3.Lerp(SpotLight.transform.localPosition, playerLampSpotLight.SpotLight.transform.localPosition, Time.deltaTime * LerpSpeed / 10);
            //else
            //    SpotLight.transform.localPosition = Vector3.Lerp(SpotLight.transform.localPosition, originalSpotLightLocalPosition, Time.deltaTime * LerpSpeed / 10);
        }
    }
}